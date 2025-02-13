using CleanSharpArchitecture.Application.DTOs.Notifications;
using CleanSharpArchitecture.Application.DTOs.Notifications.Requests;
using CleanSharpArchitecture.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSharpArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService) => _notificationService = notificationService;

        [HttpPost] // POST: api/Notification
        public async Task<NotificationDto> CreateNotification(CreateNotificationDto notificationDto)
        {
            return await _notificationService.CreateNotification(notificationDto);
        }

        [HttpPut] // PUT: api/Notification
        public async Task<NotificationDto> UpdateNotification(UpdateNotificationDto notificationDto)
        {
            return await _notificationService.UpdateNotification(notificationDto);
        }

        [HttpGet] // GET: api/Notification
        public async Task<IEnumerable<NotificationDto>> GetAllNotifications([FromQuery] int pageNumber = 1, int pageSize = 10)
        {
            return await _notificationService.GetAllNotifications(pageNumber, pageSize);
        }

        [HttpGet("{id}")] // GET: api/Notification/{id}
        public async Task<NotificationDto?> GetNotificationById(Guid id)
        {
            return await _notificationService.GetNotificationById(id);
        }
    }
}
