namespace CleanSharpArchitecture.Application.DTOs.Likes.Request
{
    public class CreateLikeDto
    {
        /// <summary>
        /// ID do usuário que está dando o like.
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// ID do post que está sendo curtido.
        /// </summary>
        public long PostId { get; set; }
    }
}
