namespace CleanSharpArchitecture.Application.DTOs.Notifications
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
