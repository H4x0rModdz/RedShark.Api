namespace CleanSharpArchitecture.Application.DTOs.Likes.Request
{
    public class CreateLikeDto
    {
        /// <summary>
        /// ID do usuário que está dando o like.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// ID do post que está sendo curtido.
        /// </summary>
        public Guid PostId { get; set; }
    }
}
