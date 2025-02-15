﻿namespace CleanSharpArchitecture.Application.DTOs.Notifications.Requests
{
    public class GetNotificationDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
