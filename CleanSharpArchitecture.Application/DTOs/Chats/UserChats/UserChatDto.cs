namespace CleanSharpArchitecture.Application.DTOs.Chats.UserChats
{
    public class UserChatDto
    {
        public long Id { get; set; }
        public long ChatId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string ChatName { get; set; }
    }
}
