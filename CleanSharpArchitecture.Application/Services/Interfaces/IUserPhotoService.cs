using CleanSharpArchitecture.Application.DTOs.UserPhotos.Requests;
using CleanSharpArchitecture.Application.DTOs.UserPhotos.Responses;
using CleanSharpArchitecture.Domain.Entities;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface IUserPhotoService
    {
        Task<IEnumerable<UserPhotoDto>> GetUserPhotos(long userId);
        Task<UserPhotoDto?> GetUserPhotoById(long photoId);
        Task<UserPhotoDto> CreateUserPhoto(long userId, CreateUserPhotoDto createDto);
        Task<UserPhotoDto> UpdateUserPhoto(long photoId, long userId, UpdateUserPhotoDto updateDto);
        Task<bool> DeleteUserPhoto(long photoId, long userId);
    }
}