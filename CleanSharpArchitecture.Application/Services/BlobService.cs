using Azure.Storage.Blobs;
using CleanSharpArchitecture.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CleanSharpArchitecture.Infrastructure.Services
{
    public class BlobService : IBlobService
    {
        private readonly IBlobClientFactory _blobClientFactory;

        public BlobService(IBlobClientFactory blobClientFactory)
        {
            _blobClientFactory = blobClientFactory;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string containerName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File must not be null or empty.", nameof(file));

            var blobContainerClient = _blobClientFactory.GetContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(file.FileName));

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            return blobClient.Uri.ToString();
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                throw new ArgumentException("File URL must not be null or empty.", nameof(fileUrl));

            var blobClient = new BlobClient(new Uri(fileUrl));
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
