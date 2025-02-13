namespace CleanSharpArchitecture.Application.DTOs.Notifications.Requests
{
    public class UpdateNotificationDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
    }
}
