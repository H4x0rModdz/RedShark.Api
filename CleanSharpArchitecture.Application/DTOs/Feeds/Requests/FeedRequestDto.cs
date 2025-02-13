namespace CleanSharpArchitecture.Application.DTOs.Feeds.Requests
{
    /// <summary>
    /// DTO contendo os parâmetros para recuperação do feed.
    /// </summary>
    public class FeedRequestDto
    {
        /// <summary>
        /// Opcional: ID do usuário para personalizar o feed. Caso não informado, pode ser extraído dos Claims.
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Cursor para paginação (pode ser um token, ou usado para indicar a posição atual).
        /// </summary>
        public string? Cursor { get; set; }

        /// <summary>
        /// Número da página a ser retornada (caso a paginação seja baseada em números).
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Quantidade de itens por página.
        /// </summary>
        public int PageSize { get; set; } = 20;
    }
}
