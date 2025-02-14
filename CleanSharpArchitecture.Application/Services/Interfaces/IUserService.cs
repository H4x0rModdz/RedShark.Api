using CleanSharpArchitecture.Application.DTOs.Users.Requests;
using CleanSharpArchitecture.Application.DTOs.Users.Responses;
using CleanSharpArchitecture.Domain.Enums;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsers(EntityStatus? status = null, string? include = null, int pageNumber = 0, int pageSize = 0);
        Task<UserDto?> GetUserById(long id);
        Task<UpdateUserResultDto> UpdateUser(UpdateUserDto updateUserDto);
        Task<DeleteUserResultDto> DeleteUser(long id);
    }
}
