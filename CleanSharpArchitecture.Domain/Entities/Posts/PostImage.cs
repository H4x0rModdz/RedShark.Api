namespace CleanSharpArchitecture.Domain.Entities.Posts
{
    public class PostImage : BaseEntity
    {
        public string ImageUrl { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
