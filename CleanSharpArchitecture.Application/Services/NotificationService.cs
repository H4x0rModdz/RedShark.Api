using AutoMapper;
using CleanSharpArchitecture.Application.DTOs.Notifications;
using CleanSharpArchitecture.Application.DTOs.Notifications.Requests;
using CleanSharpArchitecture.Application.Hubs;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CleanSharpArchitecture.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationRepository repository,
            IUserRepository userRepository,
            IHubContext<NotificationHub> hubContext,
            IMapper mapper,
            ILogger<NotificationService> logger)
        {
            _repository = repository;
            _userRepository = userRepository;
            _hubContext = hubContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<NotificationDto> CreateNotification(CreateNotificationDto dto)
        {
            var user = await _userRepository.SelectById(dto.UserId) ?? throw new Exception("User not Found.");

            var notification = new Notification
            {
                Content = dto.Content,
                IsRead = false,
                UserId = dto.UserId
            };

            var createdNotification = await _repository.CreateNotificationAsync(notification);

            var result = _mapper.Map<NotificationDto>(createdNotification);

            _logger.LogInformation("🚀 Sending SignalR notification to user_{UserId}: {Content}", dto.UserId, result.Content);
            
            try 
            {
                await _hubContext.Clients.Group($"user_{dto.UserId}").SendAsync("ReceiveNotification", result.Content);
                _logger.LogInformation("✅ Group notification sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to send SignalR notification");
            }

            return result;
        }

        public async Task<NotificationDto> UpdateNotification(UpdateNotificationDto dto, long currentUserId)
        {
            var n = await _repository.GetNotificationByIdAsync(dto.Id)
                     ?? throw new KeyNotFoundException("Notification not found.");

            // Validação de ownership: usuário só pode modificar suas próprias notificações
            if (n.UserId != currentUserId)
            {
                _logger.LogWarning("User {CurrentUserId} attempted to modify notification {NotificationId} owned by {OwnerId}", 
                    currentUserId, dto.Id, n.UserId);
                throw new UnauthorizedAccessException("Access denied: Cannot modify another user's notification.");
            }

            // Usuário pode sempre alterar IsRead
            if (dto.IsRead.HasValue) 
                n.IsRead = dto.IsRead.Value;

            // Para alterar Content, precisa ter sido criado por ele (same check já feito acima)
            if (dto.Content != null) 
                n.Content = dto.Content;

            n.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateNotificationAsync(n);

            var result = _mapper.Map<NotificationDto>(n);

            await _hubContext.Clients.Group($"user_{n.UserId}")
                .SendAsync("ReceiveNotificationUpdate", result);

            return result;
        }


        public async Task<IEnumerable<NotificationDto>> GetAllNotifications(int pageNumber, int pageSize)
        {
            var notifications = await _repository.GetAllNotificationsAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        }

        public async Task<NotificationDto> GetNotificationById(long id)
        {
            var notification = await _repository.GetNotificationByIdAsync(id);
            return notification != null ? _mapper.Map<NotificationDto>(notification) : null;
        }
    }
}
