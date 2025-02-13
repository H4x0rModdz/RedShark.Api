namespace CleanSharpArchitecture.Application.DTOs.Likes
{
    public class LikeDto
    {
        /// <summary>
        /// ID do like.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// ID do usuário que deu o like.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Nome do usuário que deu o like.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// ID do post curtido.
        /// </summary>
        public Guid PostId { get; set; }
    }
}
