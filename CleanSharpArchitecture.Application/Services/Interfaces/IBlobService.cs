using Microsoft.AspNetCore.Http;

namespace CleanSharpArchitecture.Application.Services.Interfaces
{
    public interface IBlobService
    {
        /// <summary>
        /// Faz o upload de um arquivo para o serviço de armazenamento de blobs.
        /// </summary>
        /// <param name="file">O arquivo a ser enviado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo a URL do arquivo enviado.</returns>
        Task<string> UploadFileAsync(IFormFile file, string containerName);
    }
}
