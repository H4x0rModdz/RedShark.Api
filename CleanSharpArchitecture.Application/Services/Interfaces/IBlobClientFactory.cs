using Azure.Storage.Blobs;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface IBlobClientFactory
    {
        BlobContainerClient GetContainerClient(string containerName);
    }
}
