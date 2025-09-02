using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Application.DTOs.Auth.Request;
using CleanSharpArchitecture.Application.DTOs.Auth.Response;
using CleanSharpArchitecture.Domain.Interfaces;
using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace CleanSharpArchitecture.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly BlobService _blobService;

        public AuthService(IUserRepository userRepository, ITokenService tokenService, BlobService blobService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _tokenService = tokenService;
            _blobService = blobService;
        }

        public async Task<RegisterResultDto> Register(RegisterDto registerDto)
        {
            try
            {
                Log.Information("Starting user registration for email: {Email}", registerDto.Email);
                
                await ValidateEmailAsync(registerDto.Email);
                ValidatePassword(registerDto.Password);
                await ValidateUniqueUserNameAsync(registerDto.UserName);

                var imageUrl = await UploadProfileImageAsync(registerDto.ProfileImage);
                var newUser = await CreateUserAsync(registerDto, imageUrl);

                Log.Information("User {Name} (ID: {UserId}) registered successfully", newUser.Name, newUser.Id);

                return new RegisterResultDto
                {
                    Success = true,
                    Errors = new List<string>()
                };
            }
            catch (InvalidOperationException ex)
            {
                Log.Warning("Registration failed for {Email}: {Error}", registerDto.Email, ex.Message);
                return new RegisterResultDto
                {
                    Success = false,
                    Errors = new List<string> { ex.Message }
                };
            }
            catch (ArgumentException ex)
            {
                Log.Warning("Invalid registration data for {Email}: {Error}", registerDto.Email, ex.Message);
                return new RegisterResultDto
                {
                    Success = false,
                    Errors = new List<string> { ex.Message }
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected error during registration for {Email}", registerDto.Email);
                return new RegisterResultDto
                {
                    Success = false,
                    Errors = new List<string> { "An unexpected error occurred. Please try again." }
                };
            }
        }

        public async Task<LoginResultDto> Login(LoginDto loginDto)
        {
            try
            {
                Log.Information("Login attempt for email: {Email}", loginDto.Email);
                
                var user = await _userRepository.SelectByEmail(loginDto.Email);
                if (user == null)
                {
                    Log.Warning("Login failed - user not found for email: {Email}", loginDto.Email);
                    return new LoginResultDto
                    {
                        Success = false,
                        Errors = new List<string> { "Invalid email or password." }
                    };
                }

                // Check if account is locked
                if (user.IsAccountLocked())
                {
                    Log.Warning("Login failed - account locked for user: {UserId} until {LockedUntil}", user.Id, user.LockedUntil);
                    return new LoginResultDto
                    {
                        Success = false,
                        Errors = new List<string> { $"Account is locked until {user.LockedUntil:yyyy-MM-dd HH:mm} UTC." }
                    };
                }

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                {
                    user.RegisterFailedAttempt();
                    await _userRepository.Update(user);

                    Log.Warning("Login failed - invalid password for user: {UserId}. Failed attempts: {FailedAttempts}", 
                        user.Id, user.FailedLoginAttempts);

                    var errorMessage = user.IsAccountLocked() 
                        ? "Too many failed attempts. Account has been locked for 30 minutes."
                        : "Invalid email or password.";

                    return new LoginResultDto
                    {
                        Success = false,
                        Errors = new List<string> { errorMessage }
                    };
                }

                user.ResetFailedAttempts();
                await _userRepository.Update(user);

                var token = _tokenService.GenerateToken(user);

                Log.Information("User {UserId} logged in successfully", user.Id);

                return new LoginResultDto
                {
                    Id = user.Id,
                    Success = true,
                    Token = token,
                    Name = user.Name,
                    UserName = user.UserName,
                    Email = user.Email,
                    ProfileImageUrl = user.ProfileImageUrl,
                    Biography = user.Biography,
                    Errors = new List<string>()
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected error during login for {Email}", loginDto.Email);
                return new LoginResultDto
                {
                    Success = false,
                    Errors = new List<string> { "An unexpected error occurred. Please try again." }
                };
            }
        }

        public async Task<SendCodeResultDto> SendRecoveryCode(SendRecoveryCodeRequestDto recoveryDto)
        {
            try
            {
                Log.Information("Password recovery code requested for email: {Email}", recoveryDto.Email);
                
                var user = await _userRepository.SelectByEmail(recoveryDto.Email);
                if (user == null)
                {
                    Log.Warning("Recovery code requested for non-existent email: {Email}", recoveryDto.Email);
                    // Don't reveal if user exists or not for security
                    return new SendCodeResultDto
                    {
                        Success = true,
                        Errors = new List<string>()
                    };
                }

                var recoveryCode = GenerateSecureRecoveryCode();

                user.RecoveryCode = recoveryCode;
                user.RecoveryCodeExpiration = DateTime.UtcNow.AddMinutes(15);
                await _userRepository.Update(user);

                var emailBody = $"Seu código de recuperação é: <b>{recoveryCode}</b>. Ele expira em 15 minutos.<br><br>" +
                              $"Se você não solicitou este código, ignore este email.";
                
                await _emailService.SendEmail(recoveryDto.Email, "Red-Shark - Recuperação de Senha", emailBody);

                Log.Information("Recovery code sent successfully for user: {UserId}", user.Id);

                return new SendCodeResultDto
                {
                    Success = true,
                    // Don't return the actual code in production
                    Code = null,
                    Errors = new List<string>()
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error sending recovery code for {Email}", recoveryDto.Email);
                return new SendCodeResultDto
                {
                    Success = false,
                    Errors = new List<string> { "Unable to send recovery code. Please try again later." }
                };
            }
        }

        public async Task<RecoveryResultDto> RecoveryPassword(SendRecoveryRequestDto recoveryDto)
        {
            try
            {
                Log.Information("Password recovery attempt with code: {Code}", recoveryDto.Code?.Substring(0, 2) + "****");
                
                var user = await _userRepository.SelectByRecoveryCode(recoveryDto.Code);
                if (user is null || user.RecoveryCodeExpiration < DateTime.UtcNow)
                {
                    Log.Warning("Invalid or expired recovery code used: {Code}", recoveryDto.Code?.Substring(0, 2) + "****");
                    return new RecoveryResultDto
                    {
                        Success = false,
                        Errors = new List<string> { "Código inválido ou expirado." }
                    };
                }

                // Validate new password
                try
                {
                    ValidatePassword(recoveryDto.NewPassword);
                }
                catch (ArgumentException ex)
                {
                    Log.Warning("Invalid new password format during recovery for user: {UserId}", user.Id);
                    return new RecoveryResultDto
                    {
                        Success = false,
                        Errors = new List<string> { ex.Message }
                    };
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(recoveryDto.NewPassword);
                user.RecoveryCode = null;
                user.RecoveryCodeExpiration = null;
                user.ResetFailedAttempts(); // Reset any failed login attempts
                user.UnlockAccount(); // Unlock account if it was locked
                
                await _userRepository.Update(user);

                Log.Information("Password recovered successfully for user: {UserId}", user.Id);

                return new RecoveryResultDto
                {
                    Success = true,
                    Errors = new List<string>()
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected error during password recovery");
                return new RecoveryResultDto
                {
                    Success = false,
                    Errors = new List<string> { "An unexpected error occurred. Please try again." }
                };
            }
        }

        private string GenerateSecureRecoveryCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[6];
            rng.GetBytes(bytes);
            
            return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
        }

        private async Task<string> UploadProfileImageAsync(IFormFile profileImage)
        {
            return await _blobService.UploadFileAsync(profileImage, "user-profile");
        }

        private async Task<User> CreateUserAsync(RegisterDto registerDto, string imageUrl)
        {
            var newUser = new User
            {
                UserName = Username.Create(registerDto.UserName?.Trim()),
                Name = registerDto.Name?.Trim(),
                Email = Email.Create(registerDto.Email?.ToLowerInvariant().Trim()),
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                ProfileImageUrl = imageUrl ?? "https://github.com/shadcn.png",
                Biography = string.IsNullOrWhiteSpace(registerDto.Biography) 
                    ? "This user has not provided a biography yet." 
                    : registerDto.Biography.Trim(),
            };

            return await _userRepository.Create(newUser);
        }

        private async Task ValidateEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");

            // More robust email validation
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);
            if (!emailRegex.IsMatch(email))
                throw new ArgumentException("Invalid email format.");

            var existentUser = await _userRepository.SelectByEmail(email.ToLowerInvariant().Trim());
            if (existentUser != null)
                throw new InvalidOperationException("The email is already in use.");
        }

        private async Task ValidateUniqueUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Username is required.");

            // Basic username validation
            if (userName.Length < 3 || userName.Length > 30)
                throw new ArgumentException("Username must be between 3 and 30 characters.");

            var usernameRegex = new Regex(@"^[a-zA-Z0-9_.-]+$", RegexOptions.Compiled);
            if (!usernameRegex.IsMatch(userName))
                throw new ArgumentException("Username can only contain letters, numbers, dots, hyphens, and underscores.");

            // Check for unique username (assuming you have this method)
            // var existentUser = await _userRepository.SelectByUserName(userName.Trim());
            // if (existentUser != null)
            //     throw new InvalidOperationException("The username is already in use.");
        }

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.");

            var errors = new List<string>();

            if (password.Length < 8)
                errors.Add("Password must be at least 8 characters long.");
            
            if (password.Length > 128)
                errors.Add("Password must be no more than 128 characters long.");
            
            if (!password.Any(char.IsUpper))
                errors.Add("Password must contain at least one uppercase letter.");
            
            if (!password.Any(char.IsLower))
                errors.Add("Password must contain at least one lowercase letter.");
            
            if (!password.Any(char.IsDigit))
                errors.Add("Password must contain at least one digit.");
            
            if (!password.Any(ch => "!@#$%^&*?._-+=".Contains(ch)))
                errors.Add("Password must contain at least one special character (!@#$%^&*?._-+=).");

            // Check for common weak passwords
            var commonPasswords = new[] { "password", "123456789", "qwerty123", "admin123" };
            if (commonPasswords.Any(common => password.ToLowerInvariant().Contains(common)))
                errors.Add("Password contains common weak patterns.");

            if (errors.Any())
                throw new ArgumentException(string.Join(" ", errors));
        }
    }
}