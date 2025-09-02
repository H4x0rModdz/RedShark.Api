using CleanSharpArchitecture.Application.DTOs.Chats;
using CleanSharpArchitecture.Application.DTOs.Chats.ChatMessages;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface IChatService
    {
        Task<ChatDto> CreateChat(string name, long creatorId);
        Task<ChatDto?> GetChatById(long chatId, long userId);
        Task<IEnumerable<ChatDto>> GetUserChats(long userId);
        Task<ChatDto?> GetOrCreatePrivateChat(long userId1, long userId2);
        Task<ChatMessageDto> SendMessage(long chatId, long userId, string content);
        Task<IEnumerable<ChatMessageDto>> GetChatMessages(long chatId, long userId, int pageNumber = 1, int pageSize = 50);
        Task<bool> AddUserToChat(long chatId, long userId, long requestingUserId);
        Task<bool> RemoveUserFromChat(long chatId, long userId, long requestingUserId);
    }
}