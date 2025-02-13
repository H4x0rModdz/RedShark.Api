using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Interfaces;
using CleanSharpArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanSharpArchitecture.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification> UpdateNotificationAsync(Notification notification)
        {
            var existingNotification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == notification.Id) ?? throw new Exception("Notificação não encontrada");

            existingNotification.Content = notification.Content;
            existingNotification.IsRead = notification.IsRead;
            existingNotification.UpdatedAt = DateTime.Now;
            existingNotification.Status = notification.Status;

            await _context.SaveChangesAsync();
            return existingNotification;
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync(int pageNumber, int pageSize)
        {
            return await _context.Notifications
                .OrderByDescending(n => n.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(Guid id) => await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
    }
}
