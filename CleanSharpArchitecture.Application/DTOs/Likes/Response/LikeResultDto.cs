namespace CleanSharpArchitecture.Application.DTOs.Likes.Response
{
    public class LikeResultDto
    {
        /// <summary>
        /// Indica se a operação foi realizada com sucesso.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// ID do like criado (caso a operação tenha sido de criação).
        /// </summary>
        public Guid? LikeId { get; set; }
        /// <summary>
        /// Lista de erros, se houver.
        /// </summary>
        public List<string> Errors { get; set; }
    }
}
