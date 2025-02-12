namespace CleanSharpArchitecture.Application.DTOs.Notifications.Requests
{
    public class CreateNotificationDto
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
    }
}
