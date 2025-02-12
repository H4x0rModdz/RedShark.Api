namespace CleanSharpArchitecture.Domain.Entities
{
    public class UserInterest : BaseEntity
    {
        public Guid UserId { get; set; }
        public List<Interest> Interests { get; set; }
    }
}
