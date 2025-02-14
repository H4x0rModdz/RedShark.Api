namespace CleanSharpArchitecture.Application.DTOs.Followers
{
    public class FollowerDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long FollowerId { get; set; }
        public string UserName { get; set; }
        public string FollowerName { get; set; }
    }
}
