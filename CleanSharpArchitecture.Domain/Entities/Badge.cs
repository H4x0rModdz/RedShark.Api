namespace CleanSharpArchitecture.Domain.Entities
{
    public class Badge : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
    }
}
