namespace CleanSharpArchitecture.Application.DTOs.Chats.ChatMessages
{
    /// <summary>
    /// DTO específico para SignalR que usa strings para IDs grandes
    /// Evita problemas de precisão no JavaScript (Number.MAX_SAFE_INTEGER)
    /// </summary>
    public class ChatMessageSignalRDto
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string ChatId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string UserProfileImage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}