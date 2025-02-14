namespace CleanSharpArchitecture.Domain.Entities.Chats
{
    public class ChatMessage : BaseEntity
    {
        public string Content { get; set; }
        public long ChatId { get; set; }
        public Chat Chat { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
