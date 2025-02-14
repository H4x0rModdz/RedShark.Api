namespace CleanSharpArchitecture.Application.DTOs.Chats.ChatMessages
{
    public class ChatMessageDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public long ChatId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
