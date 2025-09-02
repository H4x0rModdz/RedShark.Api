namespace CleanSharpArchitecture.Application.DTOs.Likes.Response
{
    public class LikeResultDto
    {
        /// <summary>
        /// Indicates if the operation was performed successfully.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// ID of the created like (if the operation was a creation).
        /// </summary>
        public long? LikeId { get; set; }
        /// <summary>
        /// Indicates if the post is liked after the operation.
        /// </summary>
        public bool IsLiked { get; set; }
        /// <summary>
        /// Total number of likes for the post after the operation.
        /// </summary>
        public int LikesCount { get; set; }
        /// <summary>
        /// List of errors, if any.
        /// </summary>
        public List<string> Errors { get; set; }
    }
}
