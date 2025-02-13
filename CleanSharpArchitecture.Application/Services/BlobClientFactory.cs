using Azure.Storage.Blobs;
using CleanSharpArchitecture.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CleanSharpArchitecture.Infrastructure.Services
{
    public class BlobClientFactory : IBlobClientFactory
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public BlobClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration["AzureBlobStorage:ConnectionString"];
        }

        public BlobContainerClient GetContainerClient(string containerName)
        {
            if (string.IsNullOrWhiteSpace(containerName))
                throw new ArgumentNullException(nameof(containerName), "Container name must be provided.");

            return new BlobContainerClient(_connectionString, containerName);
        }
    }
}
