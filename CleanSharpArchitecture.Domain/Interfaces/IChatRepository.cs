using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Entities.Chats;

namespace CleanSharpArchitecture.Domain.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat> CreateChat(Chat chat);
        Task<Chat?> GetChatById(long chatId);
        Task<IEnumerable<Chat>> GetUserChats(long userId);
        Task<Chat?> GetPrivateChat(long userId1, long userId2);
        Task<ChatMessage> SendMessage(ChatMessage message);
        Task<IEnumerable<ChatMessage>> GetChatMessages(long chatId, int pageNumber = 1, int pageSize = 50);
        Task AddUserToChat(long chatId, long userId);
        Task RemoveUserFromChat(long chatId, long userId);
    }
}