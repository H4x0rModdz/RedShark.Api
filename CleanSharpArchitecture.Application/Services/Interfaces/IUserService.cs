using CleanSharpArchitecture.Application.DTOs.Users.Requests;
using CleanSharpArchitecture.Application.DTOs.Users.Responses;
using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsers(EntityStatus? status = null, string? include = null, int pageNumber = 0, int pageSize = 0);
        Task<UserDto?> GetUserById(long id);
        Task<UserProfileDto?> GetUserProfile(string userName, CancellationToken cancellationToken = default);
        Task<UpdateUserResultDto> UpdateUser(UpdateUserDto updateUserDto, long currentUserId);
        Task<DeleteUserResultDto> DeleteUser(long id, long currentUserId);
        
        // UserPhoto methods
        Task<IEnumerable<UserPhoto>> GetUserPhotos(long userId);
        Task<UserPhoto?> GetUserPhotoById(long photoId);
        Task<UserPhoto> CreateUserPhoto(long userId, string imageUrl, string? description = null);
        Task<UserPhoto> UpdateUserPhoto(long photoId, long userId, string? imageUrl = null, string? description = null);
        Task<bool> DeleteUserPhoto(long photoId, long userId);
        
    }
}
