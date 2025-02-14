using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;

namespace CleanSharpArchitecture.Application.Repositories.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Cria um novo usuário no repositório.
        /// </summary>
        /// <param name="user">O usuário a ser criado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo o usuário criado.</returns>
        Task<User> Create(User user);

        Task<IEnumerable<User>> GetAllUsers(EntityStatus? status = null, string? include = null, int pageNumber = 0, int pageSize = 0);

        /// <summary>
        /// Seleciona um usuário pelo seu e-mail.
        /// </summary>
        /// <param name="email">O e-mail do usuário a ser buscado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo o usuário encontrado ou null.</returns>
        Task<User> SelectByEmail(string email);

        /// <summary>
        /// Seleciona um usuário pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do usuário a ser buscado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo o usuário encontrado ou null.</returns>
        Task<User> SelectById(long id);

        /// <summary>
        /// Atualiza as informações de um usuário existente.
        /// </summary>
        /// <param name="user">O usuário com as informações atualizadas.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task Update(User user);

        /// <summary>
        /// Remove um usuário do repositório.
        /// </summary>
        /// <param name="id">O ID do usuário a ser removido.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task Delete(long id);


        /// <summary>
        /// Seleciona um usuário pelo seu código de recuperação de senha.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task<User> SelectByRecoveryCode(string code);
    }
}