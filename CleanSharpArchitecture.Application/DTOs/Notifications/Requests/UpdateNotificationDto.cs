namespace CleanSharpArchitecture.Application.DTOs.Notifications.Requests
{
    public class UpdateNotificationDto
    {
        public long Id { get; set; }
        public string? Content { get; set; }
        public bool? IsRead { get; set; }
    }
}
