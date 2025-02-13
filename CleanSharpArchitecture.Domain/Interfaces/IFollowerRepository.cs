namespace CleanSharpArchitecture.Domain.Interfaces
{
    public interface IFollowerRepository
    {
        /// <summary>
        /// Retorna a lista de IDs dos usuários que o usuário (follower) está seguindo.
        /// </summary>
        Task<IEnumerable<Guid>> GetFollowedUserIds(Guid followerId);
    }
}
