using CleanSharpArchitecture.Application.DTOs.Auth.Request;
using CleanSharpArchitecture.Application.DTOs.Auth.Response;
using CleanSharpArchitecture.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanSharpArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        public async Task<RegisterResultDto> Register([FromForm] RegisterDto registerUserDto)
        {
            return await _authService.Register(registerUserDto);
        }

        [HttpPost("login")]
        public async Task<LoginResultDto> Login([FromBody] LoginDto loginDto)
        {
            return await _authService.Login(loginDto);
        }

        [HttpPost("send-code")]
        public async Task<SendCodeResultDto> SendRecoveryCode([FromBody] SendRecoveryCodeRequestDto recoveryDto)
        {
            return await _authService.SendRecoveryCode(recoveryDto);
        }

        [HttpPost("recovery-password")]
        public async Task<RecoveryResultDto> RecoveryPassword([FromBody] SendRecoveryRequestDto recoveryDto)
        {
            return await _authService.RecoveryPassword(recoveryDto);
        }
    }
}