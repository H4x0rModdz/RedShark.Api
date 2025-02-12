using CleanSharpArchitecture.Application.DTOs.Posts.Request;

namespace CleanSharpArchitecture.Application.DTOs.Feeds.Responses
{
    /// <summary>
    /// DTO contendo os dados do feed, incluindo a lista de posts e informações para paginação.
    /// </summary>
    public class FeedResponseDto
    {
        /// <summary>
        /// Lista de posts que compõem o feed.
        /// </summary>
        public IEnumerable<PostDto> Posts { get; set; } = new List<PostDto>();

        /// <summary>
        /// Cursor para a próxima página de resultados. Pode ser nulo caso não haja mais itens.
        /// </summary>
        public string? NextCursor { get; set; }
    }
}
