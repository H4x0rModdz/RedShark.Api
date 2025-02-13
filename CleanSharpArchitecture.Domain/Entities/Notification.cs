namespace CleanSharpArchitecture.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
