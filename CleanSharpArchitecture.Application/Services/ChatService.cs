using AutoMapper;
using CleanSharpArchitecture.Application.DTOs.Chats;
using CleanSharpArchitecture.Application.DTOs.Chats.ChatMessages;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Entities.Chats;
using CleanSharpArchitecture.Domain.Enums;
using CleanSharpArchitecture.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using CleanSharpArchitecture.Application.Hubs;

namespace CleanSharpArchitecture.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ChatService(IChatRepository chatRepository, IMapper mapper, IHubContext<NotificationHub> hubContext)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<ChatDto> CreateChat(string name, long creatorId)
        {
            try
            {
                var chat = new Chat
                {
                    Name = name,
                    Status = EntityStatus.Active
                };

                var createdChat = await _chatRepository.CreateChat(chat);
                
                await _chatRepository.AddUserToChat(createdChat.Id, creatorId);

                var chatWithParticipants = await _chatRepository.GetChatById(createdChat.Id);
                return _mapper.Map<ChatDto>(chatWithParticipants);
            }
            catch (Exception ex)
            {
                Log.Error($"Error creating chat: {ex.Message}");
                throw;
            }
        }

        public async Task<ChatDto?> GetChatById(long chatId, long userId)
        {
            try
            {
                var chat = await _chatRepository.GetChatById(chatId);
                
                if (chat == null || !chat.Participants.Any(p => p.UserId == userId))
                {
                    return null;
                }

                return _mapper.Map<ChatDto>(chat);
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting chat by id: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ChatDto>> GetUserChats(long userId)
        {
            try
            {
                var chats = await _chatRepository.GetUserChats(userId);
                return _mapper.Map<IEnumerable<ChatDto>>(chats);
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting user chats: {ex.Message}");
                throw;
            }
        }

        public async Task<ChatDto?> GetOrCreatePrivateChat(long userId1, long userId2)
        {
            try
            {
                var existingChat = await _chatRepository.GetPrivateChat(userId1, userId2);
                
                if (existingChat != null)
                {
                    return _mapper.Map<ChatDto>(existingChat);
                }

                var chat = new Chat
                {
                    Name = $"Private Chat",
                    Status = EntityStatus.Active
                };

                var createdChat = await _chatRepository.CreateChat(chat);
                
                await _chatRepository.AddUserToChat(createdChat.Id, userId1);
                await _chatRepository.AddUserToChat(createdChat.Id, userId2);

                var chatWithParticipants = await _chatRepository.GetChatById(createdChat.Id);
                return _mapper.Map<ChatDto>(chatWithParticipants);
            }
            catch (Exception ex)
            {
                Log.Error($"Error creating private chat: {ex.Message}");
                throw;
            }
        }

        public async Task<ChatMessageDto> SendMessage(long chatId, long userId, string content)
        {
            try
            {
                var chat = await _chatRepository.GetChatById(chatId);
                
                if (chat == null || !chat.Participants.Any(p => p.UserId == userId))
                {
                    throw new UnauthorizedAccessException("User is not part of this chat");
                }

                var message = new ChatMessage
                {
                    Content = content,
                    ChatId = chatId,
                    UserId = userId,
                    Status = EntityStatus.Active
                };

                var sentMessage = await _chatRepository.SendMessage(message);
                var messageDto = _mapper.Map<ChatMessageDto>(sentMessage);

                try
                {
                    // Create SignalR-specific DTO with string IDs to avoid JavaScript precision issues
                    var signalRDto = new CleanSharpArchitecture.Application.DTOs.Chats.ChatMessages.ChatMessageSignalRDto
                    {
                        Id = sentMessage.Id.ToString(),
                        Content = sentMessage.Content,
                        ChatId = sentMessage.ChatId.ToString(),
                        UserId = sentMessage.UserId.ToString(),
                        Name = sentMessage.User?.Name ?? "User",
                        UserName = sentMessage.User?.UserName?.ToString() ?? "@user",
                        UserProfileImage = sentMessage.User?.ProfileImageUrl ?? "https://github.com/shadcn.png",
                        CreatedAt = sentMessage.CreatedAt
                    };

                    // Send to chat group
                    await _hubContext.Clients.Group($"chat_{chatId}")
                        .SendAsync("ReceiveChatMessage", signalRDto);
                    
                    // Also send to individual users (like notifications)
                    foreach (var participant in chat.Participants)
                    {
                        await _hubContext.Clients.Group($"user_{participant.UserId}")
                            .SendAsync("ReceiveChatMessage", signalRDto);
                    }
                }
                catch (Exception ex)
                {
                    Log.Warning($"Failed to send real-time message: {ex.Message}");
                }

                return messageDto;
            }
            catch (Exception ex)
            {
                Log.Error($"Error sending message: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ChatMessageDto>> GetChatMessages(long chatId, long userId, int pageNumber = 1, int pageSize = 50)
        {
            try
            {
                var chat = await _chatRepository.GetChatById(chatId);
                
                if (chat == null || !chat.Participants.Any(p => p.UserId == userId))
                {
                    throw new UnauthorizedAccessException("User is not part of this chat");
                }

                var messages = await _chatRepository.GetChatMessages(chatId, pageNumber, pageSize);
                return _mapper.Map<IEnumerable<ChatMessageDto>>(messages);
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting chat messages: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> AddUserToChat(long chatId, long userId, long requestingUserId)
        {
            try
            {
                var chat = await _chatRepository.GetChatById(chatId);
                
                if (chat == null || !chat.Participants.Any(p => p.UserId == requestingUserId))
                {
                    return false;
                }

                await _chatRepository.AddUserToChat(chatId, userId);

                try
                {
                    await _hubContext.Clients.User(userId.ToString())
                        .SendAsync("ReceiveNotification", $"You were added to chat: {chat.Name}");
                }
                catch (Exception ex)
                {
                    Log.Warning($"Failed to send notification: {ex.Message}");
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Error adding user to chat: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveUserFromChat(long chatId, long userId, long requestingUserId)
        {
            try
            {
                var chat = await _chatRepository.GetChatById(chatId);
                
                if (chat == null || !chat.Participants.Any(p => p.UserId == requestingUserId))
                {
                    return false;
                }

                if (userId != requestingUserId)
                {
                    return false;
                }

                await _chatRepository.RemoveUserFromChat(chatId, userId);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Error removing user from chat: {ex.Message}");
                return false;
            }
        }
    }
}