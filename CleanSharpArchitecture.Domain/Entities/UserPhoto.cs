namespace CleanSharpArchitecture.Domain.Entities
{
    public class UserPhoto : BaseEntity
    {
        public string ImageUrl { get; set; }
        public string? Description { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}