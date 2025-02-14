using CleanSharpArchitecture.Domain.Entities;

namespace CleanSharpArchitecture.Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task<Notification> UpdateNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetAllNotificationsAsync(int pageNumber, int pageSize);
        Task<Notification> GetNotificationByIdAsync(long id);
    }
}
