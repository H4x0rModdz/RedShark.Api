using CleanSharpArchitecture.Application.DTOs.Auth.Request;
using CleanSharpArchitecture.Application.DTOs.Auth.Response;
using CleanSharpArchitecture.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CleanSharpArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) 
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        /// <summary>
        /// Registers a new user account
        /// </summary>
        /// <param name="registerUserDto">User registration data</param>
        /// <returns>Registration result</returns>
        /// <response code="200">User registered successfully</response>
        /// <response code="400">Invalid registration data</response>
        /// <response code="429">Too many registration attempts</response>
        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(RegisterResultDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        public async Task<ActionResult<RegisterResultDto>> Register([FromForm, Required] RegisterDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new RegisterResultDto
                {
                    Success = false,
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            var result = await _authService.Register(registerUserDto);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token
        /// </summary>
        /// <param name="loginDto">User login credentials</param>
        /// <returns>Login result with JWT token</returns>
        /// <response code="200">Login successful</response>
        /// <response code="400">Invalid credentials</response>
        /// <response code="429">Too many login attempts</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResultDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        public async Task<ActionResult<LoginResultDto>> Login([FromBody, Required] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new LoginResultDto
                {
                    Success = false,
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            var result = await _authService.Login(loginDto);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Sends a password recovery code to the user's email
        /// </summary>
        /// <param name="recoveryDto">Email for password recovery</param>
        /// <returns>Recovery code send result</returns>
        /// <response code="200">Recovery code sent (or user doesn't exist - same response for security)</response>
        /// <response code="400">Invalid email format</response>
        /// <response code="429">Too many recovery requests</response>
        [HttpPost("send-code")]
        [ProducesResponseType(typeof(SendCodeResultDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        public async Task<ActionResult<SendCodeResultDto>> SendRecoveryCode([FromBody, Required] SendRecoveryCodeRequestDto recoveryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new SendCodeResultDto
                {
                    Success = false,
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            var result = await _authService.SendRecoveryCode(recoveryDto);
            return Ok(result); // Always return 200 for security (don't reveal if user exists)
        }

        /// <summary>
        /// Recovers user password using recovery code
        /// </summary>
        /// <param name="recoveryDto">Recovery code and new password</param>
        /// <returns>Password recovery result</returns>
        /// <response code="200">Password recovered successfully</response>
        /// <response code="400">Invalid recovery code or password format</response>
        /// <response code="429">Too many recovery attempts</response>
        [HttpPost("recovery-password")]
        [ProducesResponseType(typeof(RecoveryResultDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        public async Task<ActionResult<RecoveryResultDto>> RecoveryPassword([FromBody, Required] SendRecoveryRequestDto recoveryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new RecoveryResultDto
                {
                    Success = false,
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            var result = await _authService.RecoveryPassword(recoveryDto);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}