namespace CleanSharpArchitecture.Application.DTOs.Chats.ChatMessages
{
    public class ChatMessageDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
