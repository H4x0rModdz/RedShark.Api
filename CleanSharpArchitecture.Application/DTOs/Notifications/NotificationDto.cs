using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
