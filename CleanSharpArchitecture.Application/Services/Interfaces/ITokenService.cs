using CleanSharpArchitecture.Domain.Entities;
using System.Threading.Tasks;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Gera um token JWT para o usuário fornecido.
        /// </summary>
        /// <param name="user">O usuário para o qual o token será gerado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo o token gerado.</returns>
        string GenerateToken(User user);
    }
}
