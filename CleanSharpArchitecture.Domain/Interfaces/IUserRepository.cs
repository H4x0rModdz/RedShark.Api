using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;

namespace CleanSharpArchitecture.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Create(User user);

        Task<IEnumerable<User>> GetAllUsers(EntityStatus? status = null, string? include = null, int pageNumber = 0, int pageSize = 0);

        Task<User> SelectByEmail(string email);

        Task<User> SelectById(long id);

        Task<User> SelectByUserName(string userName);

        Task Update(User user);

        Task Delete(long id);


        Task<User> SelectByRecoveryCode(string code);
    }
}