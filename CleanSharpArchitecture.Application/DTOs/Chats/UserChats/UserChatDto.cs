using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanSharpArchitecture.Application.DTOs.Chats.UserChats
{
    public class UserChatDto
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string ChatName { get; set; }
    }
}
