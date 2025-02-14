using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Application.DTOs.Auth.Request;
using CleanSharpArchitecture.Application.DTOs.Auth.Response;
using CleanSharpArchitecture.Application.Repositories.Interfaces;
using CleanSharpArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Serilog;

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
                ValidateEmail(registerDto.Email);
                ValidatePassword(registerDto.Password);

                var imageUrl = await UploadProfileImageAsync(registerDto.ProfileImage);
                var newUser = await CreateUser(registerDto, imageUrl);

                Log.Information($"User {newUser.Name} registered successfully.");

                return new RegisterResultDto
                {
                    Success = true,
                    Errors = new List<string>()
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Error registering user: {ex.Message}");
                return new RegisterResultDto
                {
                    Success = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<LoginResultDto> Login(LoginDto loginDto)
        {
            var user = await _userRepository.SelectByEmail(loginDto.Email) ?? throw new Exception("User not found.");

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                user.RegisterFailedAttempt();
                await _userRepository.Update(user);

                Log.Warning("User {Name} failed to login. Failed attempts: {FailedLoginAttempts}", user.Name, user.FailedLoginAttempts);

                throw new Exception("Invalid email or password.");
            }

            user.ResetFailedAttempts();
            await _userRepository.Update(user);

            var token = _tokenService.GenerateToken(user);

            Log.Information("User {Name} logged in successfully.", user.Name);

            return new LoginResultDto
            {
                Id = user.Id,
                Success = true,
                Token = token,
                Errors = new List<string>()
            };
        }

        public async Task<SendCodeResultDto> SendRecoveryCode(SendRecoveryCodeRequestDto recoveryDto)
        {
            var user = await _userRepository.SelectByEmail(recoveryDto.Email) ?? throw new Exception("User not found.");
            var recoveryCode = GenerateRecoveryCode();

            user.RecoveryCode = recoveryCode;
            user.RecoveryCodeExpiration = DateTime.UtcNow.AddMinutes(15);
            await _userRepository.Update(user);

            var emailBody = $"Seu código de recuperação é: <b>{recoveryCode}</b>. Ele expira em 15 minutos.";
            await _emailService.SendEmail(recoveryDto.Email, "Recuperação de Senha", emailBody);

            return new SendCodeResultDto
            {
                Success = true,
                Code = user.RecoveryCode,
                Errors = new List<string>()
            };
        }

        public async Task<RecoveryResultDto> RecoveryPassword(SendRecoveryRequestDto recoveryDto)
        {
            var user = await _userRepository.SelectByRecoveryCode(recoveryDto.Code);
            if (user is null || user.RecoveryCodeExpiration < DateTime.UtcNow)
            {
                return new RecoveryResultDto
                {
                    Success = false,
                    Errors = new List<string> { "Código inválido ou expirado." }
                };
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(recoveryDto.NewPassword);
            user.RecoveryCode = null;
            user.RecoveryCodeExpiration = null;
            await _userRepository.Update(user);

            return new RecoveryResultDto
            {
                Success = true,
                Errors = new List<string>()
            };
        }

        private string GenerateRecoveryCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async Task<string> UploadProfileImageAsync(IFormFile profileImage)
        {
            return await _blobService.UploadFileAsync(profileImage, "user-profile");
        }

        private async Task<User> CreateUser(RegisterDto registerDto, string imageUrl)
        {
            var existentUser = await _userRepository.SelectByEmail(registerDto.Email);
            if (existentUser != null)
                throw new Exception("The email is already in use.");

            var newUser = new User
            {
                UserName = registerDto.UserName,
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                ProfileImageUrl = imageUrl,
                Biography = registerDto.Biography,
            };

            return await _userRepository.Create(newUser);
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                throw new Exception("Invalid email format.");

            var existentUser = _userRepository.SelectByEmail(email).Result;
            if (existentUser != null)
                throw new Exception("The email is already in use.");
        }

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8 || password.Length > 32 ||
                !password.Any(char.IsUpper) || !password.Any(char.IsLower) ||
                !password.Any(char.IsDigit) || !password.Any(ch => "!@#$%^&*?".Contains(ch)))
            {
                throw new Exception("Invalid password format.");
            }
        }
    }
}