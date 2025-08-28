namespace CleanSharpArchitecture.Application.Exceptions
{
    public class PostNotFoundException : Exception
    {
        public PostNotFoundException() : base("Post not found.")
        {
        }

        public PostNotFoundException(string message) : base(message)
        {
        }

        public PostNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PostNotFoundException(long postId) : base($"Post with ID {postId} not found.")
        {
        }
    }
}