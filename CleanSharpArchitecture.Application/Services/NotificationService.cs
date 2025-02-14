using AutoMapper;
using CleanSharpArchitecture.Application.DTOs.Notifications;
using CleanSharpArchitecture.Application.DTOs.Notifications.Requests;
using CleanSharpArchitecture.Application.Hubs;
using CleanSharpArchitecture.Application.Repositories.Interfaces;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CleanSharpArchitecture.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMapper _mapper;

        public NotificationService(
            INotificationRepository repository,
            IUserRepository userRepository,
            IHubContext<NotificationHub> hubContext,
            IMapper mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _hubContext = hubContext;
            _mapper = mapper;
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

            await _hubContext.Clients.Group(dto.UserId.ToString()).SendAsync("ReceiveNotification", result.Content); // Send to specific client

            //await _hubContext.Clients.All.SendAsync("ReceiveNotification", "enviando para todos: " + result.Content); // Send to all clients

            return result;
        }

        public async Task<NotificationDto> UpdateNotification(UpdateNotificationDto dto)
        {
            var notification = await _repository.GetNotificationByIdAsync(dto.Id) ?? throw new Exception("Notification not found.");

            var updatedNotification = await _repository.UpdateNotificationAsync(notification);

            var result = _mapper.Map<NotificationDto>(updatedNotification);

            await _hubContext.Clients.Group(notification.UserId.ToString()).SendAsync("ReceiveNotificationUpdate", result.Content);

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
