using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanSharpArchitecture.Application.DTOs.Followers
{
    public class FollowerDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid FollowerId { get; set; }
        public string UserName { get; set; }
        public string FollowerName { get; set; }
    }
}
