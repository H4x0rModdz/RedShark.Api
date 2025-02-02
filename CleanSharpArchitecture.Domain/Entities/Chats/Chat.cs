using CleanSharpArchitecture.Domain.Entities.Chats;

namespace CleanSharpArchitecture.Domain.Entities
{
    public class Chat : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<UserChat> Participants { get; set; } = new List<UserChat>();
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}
