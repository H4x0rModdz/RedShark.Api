using CleanSharpArchitecture.Application.DTOs.Auth.Request;
using CleanSharpArchitecture.Application.DTOs.Auth.Response;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResultDto> Register(RegisterDto registerDto);
        Task<LoginResultDto> Login(LoginDto loginDto);
        Task<RecoveryResultDto> RecoveryPassword(SendRecoveryRequestDto recoveryDto);
        Task<SendCodeResultDto> SendRecoveryCode(SendRecoveryCodeRequestDto recoveryDto);
    }
}
