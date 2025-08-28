namespace CleanSharpArchitecture.Domain.Entities
{
    public class UserInterest : BaseEntity
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long InterestId { get; set; }
        public Interest Interest { get; set; }
    }
}
