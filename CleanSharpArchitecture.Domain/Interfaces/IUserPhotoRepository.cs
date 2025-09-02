using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;

namespace CleanSharpArchitecture.Domain.Interfaces
{
    public interface IUserPhotoRepository
    {
        Task<UserPhoto> Create(UserPhoto userPhoto);
        Task<UserPhoto?> GetById(long photoId);
        Task<IEnumerable<UserPhoto>> GetByUserId(long userId, int pageNumber = 1, int pageSize = 20);
        Task Update(UserPhoto userPhoto);
        Task Delete(long photoId);
        Task<IEnumerable<UserPhoto>> GetAll(long? userId, EntityStatus? status, int pageNumber = 1, int pageSize = 20);
    }
}