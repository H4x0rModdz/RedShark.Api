using Microsoft.AspNetCore.Http;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface IBlobService
    {
        /// <summary>
        /// Uploads a file to the blob storage service.
        /// </summary>
        /// <param name="file">The file to be uploaded.</param>
        /// <returns>A task representing the asynchronous operation, containing the URL of the uploaded file.</returns>
        Task<string> UploadFileAsync(IFormFile file, string containerName);
    }
}
