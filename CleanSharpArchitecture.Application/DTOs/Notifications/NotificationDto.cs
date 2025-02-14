namespace CleanSharpArchitecture.Application.DTOs.Notifications
{
    public class NotificationDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
