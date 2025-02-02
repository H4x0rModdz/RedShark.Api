using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanSharpArchitecture.Application.DTOs.Badges.UserBadges
{
    public class UserBadgeDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BadgeId { get; set; }
        public string BadgeName { get; set; }
        public string BadgeDescription { get; set; }
        public string BadgeIconUrl { get; set; }
    }
}
