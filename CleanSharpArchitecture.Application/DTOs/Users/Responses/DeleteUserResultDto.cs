namespace CleanSharpArchitecture.Application.DTOs.Users.Responses
{
    public class DeleteUserResultDto
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
