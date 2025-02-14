using CleanSharpArchitecture.Application.DTOs.Notifications;
using CleanSharpArchitecture.Application.DTOs.Notifications.Requests;
using CleanSharpArchitecture.Application.DTOs.Notifications.Responses;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationDto> CreateNotification(CreateNotificationDto dto);
        Task<NotificationDto> UpdateNotification(UpdateNotificationDto dto);
        Task<IEnumerable<NotificationDto>> GetAllNotifications(int pageNumber, int pageSize);
        Task<NotificationDto> GetNotificationById(long id);
    }
}
