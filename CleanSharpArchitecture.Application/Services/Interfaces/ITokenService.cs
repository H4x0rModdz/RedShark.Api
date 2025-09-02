using CleanSharpArchitecture.Domain.Entities;
using System.Threading.Tasks;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT token for the provided user.
        /// </summary>
        /// <param name="user">The user for whom the token will be generated.</param>
        /// <returns>A task representing the asynchronous operation, containing the generated token.</returns>
        string GenerateToken(User user);
    }
}
