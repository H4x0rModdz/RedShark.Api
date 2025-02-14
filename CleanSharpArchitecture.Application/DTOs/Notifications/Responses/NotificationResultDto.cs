using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanSharpArchitecture.Application.DTOs.Notifications.Responses
{
    public class NotificationResultDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Status { get; set; } // Ex.: "Active", "Inactive", etc.
    }
}
