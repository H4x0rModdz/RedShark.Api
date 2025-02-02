using CleanSharpArchitecture.Application.DTOs.Chats.ChatMessages;
using CleanSharpArchitecture.Application.DTOs.Chats.UserChats;

namespace CleanSharpArchitecture.Application.DTOs.Chats
{
    public class ChatDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserChatDto>? Participants { get; set; } = new List<UserChatDto>();
        public ICollection<ChatMessageDto>? Messages { get; set; } = new List<ChatMessageDto>();
    }
}
