namespace CleanSharpArchitecture.Application.DTOs.Auth.Response
{
    public class SendCodeResultDto
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public string Code { get; set; }
    }
}
