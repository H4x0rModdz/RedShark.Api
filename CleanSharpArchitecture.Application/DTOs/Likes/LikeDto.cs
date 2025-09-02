namespace CleanSharpArchitecture.Application.DTOs.Likes
{
    public class LikeDto
    {
        /// <summary>
        /// ID of the like.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// ID of the user who gave the like.
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// Name of the user who gave the like.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// ID of the liked post.
        /// </summary>
        public long PostId { get; set; }
    }
}
