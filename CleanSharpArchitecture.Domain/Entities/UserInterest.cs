namespace CleanSharpArchitecture.Domain.Entities
{
    public class UserInterest : BaseEntity
    {
        public long UserId { get; set; }
        public List<Interest> Interests { get; set; }
    }
}
