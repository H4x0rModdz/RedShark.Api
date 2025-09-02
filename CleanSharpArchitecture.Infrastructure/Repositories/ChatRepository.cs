using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Entities.Chats;
using CleanSharpArchitecture.Domain.Enums;
using CleanSharpArchitecture.Domain.Interfaces;
using CleanSharpArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanSharpArchitecture.Infrastructure.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Chat> CreateChat(Chat chat)
        {
            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task<Chat?> GetChatById(long chatId)
        {
            return await _context.Chats
                .Include(c => c.Participants)
                    .ThenInclude(p => p.User)
                .Include(c => c.Messages.OrderByDescending(m => m.CreatedAt).Take(50))
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(c => c.Id == chatId && c.Status == EntityStatus.Active);
        }

        public async Task<IEnumerable<Chat>> GetUserChats(long userId)
        {
            return await _context.Chats
                .Include(c => c.Participants)
                    .ThenInclude(p => p.User)
                .Include(c => c.Messages.OrderByDescending(m => m.CreatedAt).Take(1))
                    .ThenInclude(m => m.User)
                .Where(c => c.Participants.Any(p => p.UserId == userId) && c.Status == EntityStatus.Active)
                .OrderByDescending(c => c.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault().CreatedAt)
                .ToListAsync();
        }

        public async Task<Chat?> GetPrivateChat(long userId1, long userId2)
        {
            return await _context.Chats
                .Include(c => c.Participants)
                    .ThenInclude(p => p.User)
                .Include(c => c.Messages.OrderByDescending(m => m.CreatedAt).Take(50))
                    .ThenInclude(m => m.User)
                .Where(c => c.Participants.Count == 2 && 
                           c.Participants.Any(p => p.UserId == userId1) && 
                           c.Participants.Any(p => p.UserId == userId2) &&
                           c.Status == EntityStatus.Active)
                .FirstOrDefaultAsync();
        }

        public async Task<ChatMessage> SendMessage(ChatMessage message)
        {
            await _context.ChatMessages.AddAsync(message);
            await _context.SaveChangesAsync();
            
            return await _context.ChatMessages
                .Include(m => m.User)
                .Include(m => m.Chat)
                .FirstAsync(m => m.Id == message.Id);
        }

        public async Task<IEnumerable<ChatMessage>> GetChatMessages(long chatId, int pageNumber = 1, int pageSize = 50)
        {
            return await _context.ChatMessages
                .Include(m => m.User)
                .Where(m => m.ChatId == chatId && m.Status == EntityStatus.Active)
                .OrderByDescending(m => m.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task AddUserToChat(long chatId, long userId)
        {
            var existingUserChat = await _context.UserChats
                .FirstOrDefaultAsync(uc => uc.ChatId == chatId && uc.UserId == userId);

            if (existingUserChat == null)
            {
                var userChat = new UserChat
                {
                    ChatId = chatId,
                    UserId = userId,
                    Status = EntityStatus.Active
                };

                await _context.UserChats.AddAsync(userChat);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveUserFromChat(long chatId, long userId)
        {
            var userChat = await _context.UserChats
                .FirstOrDefaultAsync(uc => uc.ChatId == chatId && uc.UserId == userId);

            if (userChat != null)
            {
                userChat.Status = EntityStatus.Deleted;
                await _context.SaveChangesAsync();
            }
        }
    }
}