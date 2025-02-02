namespace CleanSharpArchitecture.Domain.Entities.Chats
{
    public class ChatMessage : BaseEntity
    {
        public string Content { get; set; }
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
