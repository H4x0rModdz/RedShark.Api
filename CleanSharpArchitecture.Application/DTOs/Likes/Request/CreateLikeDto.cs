namespace CleanSharpArchitecture.Application.DTOs.Likes.Request
{
    public class CreateLikeDto
    {
        /// <summary>
        /// ID of the user who is giving the like.
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// ID of the post being liked.
        /// </summary>
        public long? PostId { get; set; }
        /// <summary>
        /// ID of the comment being liked.
        /// </summary>
        public long? CommentId { get; set; }
    }
}
