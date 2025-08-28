using AutoMapper;
using CleanSharpArchitecture.Application.DTOs.Posts.Request;
using CleanSharpArchitecture.Application.DTOs.Posts.Response;
using CleanSharpArchitecture.Application.Interfaces.Repositories;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Domain.Entities.Posts;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Text.RegularExpressions;

namespace CleanSharpArchitecture.Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly BlobService _blobService;
        private readonly IMapper _mapper;
        
        // Constants for validation
        private const int MaxContentLength = 2000;
        private const int MaxImagesPerPost = 10;
        private const long MaxImageSizeBytes = 10 * 1024 * 1024; // 10MB
        private static readonly string[] AllowedImageTypes = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        public PostService(IPostRepository postRepository, BlobService blobService, IMapper mapper)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
            _blobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Cria um novo post e processa o upload das imagens.
        /// </summary>
        /// <param name="postDto">DTO contendo o conteúdo do post, o ID do usuário e os arquivos de imagem a serem enviados.</param>
        /// <returns>Retorna um objeto <see cref="PostResultDto"/> com o resultado da operação.</returns>
        public async Task<PostResultDto> CreatePost(CreatePostDto postDto)
        {
            try
            {
                Log.Information("Creating post for user: {UserId}", postDto.UserId);
                
                // Validate input
                ValidateCreatePostDto(postDto);
                await ValidateImagesAsync(postDto.Images);
                
                var post = CreatePostEntity(postDto);
                var createdPost = await _postRepository.Create(post);

                // Process images asynchronously for better performance
                var imageProcessingTask = ProcessNewPostImagesAsync(createdPost, postDto.Images);
                await imageProcessingTask;

                Log.Information("Post {PostId} created successfully for user {UserId}", createdPost.Id, postDto.UserId);

                return new PostResultDto
                {
                    Success = true,
                    PostId = createdPost.Id.ToString(),
                    Errors = new List<string>()
                };
            }
            catch (ArgumentException ex)
            {
                Log.Warning("Invalid post data for user {UserId}: {Error}", postDto.UserId, ex.Message);
                return new PostResultDto
                {
                    Success = false,
                    Errors = new List<string> { ex.Message }
                };
            }
            catch (InvalidOperationException ex)
            {
                Log.Warning("Post creation failed for user {UserId}: {Error}", postDto.UserId, ex.Message);
                return new PostResultDto
                {
                    Success = false,
                    Errors = new List<string> { ex.Message }
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected error creating post for user {UserId}", postDto.UserId);
                return new PostResultDto
                {
                    Success = false,
                    Errors = new List<string> { "An unexpected error occurred. Please try again." }
                };
            }
        }

        /// <summary>
        /// Atualiza um post existente, alterando seu conteúdo e gerenciando as imagens associadas.
        /// </summary>
        /// <param name="postDto">
        /// DTO contendo o ID do post, novo conteúdo, os IDs das imagens que devem ser mantidas e os novos arquivos de imagem a serem adicionados.
        /// </param>
        /// <returns>Retorna um objeto <see cref="PostResultDto"/> com o resultado da operação.</returns>
        public async Task<PostResultDto> UpdatePost(UpdatePostDto postDto)
        {
            try
            {
                Log.Information("Updating post {PostId}", postDto.PostId);
                
                ValidateUpdatePostDto(postDto);
                await ValidateImagesAsync(postDto.NewImages);
                
                var post = await _postRepository.GetById(postDto.PostId);
                if (post == null)
                {
                    Log.Warning("Post {PostId} not found for update", postDto.PostId);
                    return new PostResultDto
                    {
                        Success = false,
                        Errors = new List<string> { "Post not found." }
                    };
                }

                // Update content
                post.Content = postDto.Content?.Trim();

                // Handle image updates
                await RemoveImagesNotInKeepListAsync(post, postDto.ImagesToKeep);
                await AddNewImagesAsync(post, postDto.NewImages);
                
                // Validate total image count after updates
                if (post.Images.Count > MaxImagesPerPost)
                {
                    throw new InvalidOperationException($"Posts cannot have more than {MaxImagesPerPost} images.");
                }

                await _postRepository.Update(post);

                Log.Information("Post {PostId} updated successfully", post.Id);

                return new PostResultDto
                {
                    Success = true,
                    PostId = post.Id.ToString(),
                    Errors = new List<string>()
                };
            }
            catch (ArgumentException ex)
            {
                Log.Warning("Invalid update data for post {PostId}: {Error}", postDto.PostId, ex.Message);
                return new PostResultDto
                {
                    Success = false,
                    Errors = new List<string> { ex.Message }
                };
            }
            catch (InvalidOperationException ex)
            {
                Log.Warning("Update failed for post {PostId}: {Error}", postDto.PostId, ex.Message);
                return new PostResultDto
                {
                    Success = false,
                    Errors = new List<string> { ex.Message }
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected error updating post {PostId}", postDto.PostId);
                return new PostResultDto
                {
                    Success = false,
                    Errors = new List<string> { "An unexpected error occurred. Please try again." }
                };
            }
        }

        /// <summary>
        /// Exclui um post pelo seu ID.
        /// </summary>
        /// <param name="postId">ID do post a ser excluído.</param>
        /// <returns>Retorna um objeto <see cref="PostResultDto"/> com o resultado da operação.</returns>
        public async Task<PostResultDto> DeletePost(long postId)
        {
            try
            {
                Log.Information("Deleting post {PostId}", postId);
                
                var post = await _postRepository.GetById(postId);
                if (post == null)
                {
                    Log.Warning("Post {PostId} not found for deletion", postId);
                    return new PostResultDto 
                    { 
                        Success = false, 
                        Errors = new List<string> { "Post not found." } 
                    };
                }

                // Delete images from blob storage first
                await DeletePostImagesAsync(post);
                
                // Then delete the post
                await _postRepository.Delete(postId);

                Log.Information("Post {PostId} deleted successfully", postId);

                return new PostResultDto 
                { 
                    Success = true, 
                    Errors = new List<string>() 
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected error deleting post {PostId}", postId);
                return new PostResultDto 
                { 
                    Success = false, 
                    Errors = new List<string> { "An unexpected error occurred. Please try again." } 
                };
            }
        }

        /// <summary>
        /// Recupera os posts paginados e os mapeia para o DTO de exibição.
        /// </summary>
        /// <param name="pageNumber">Número da página a ser recuperada (padrão 1).</param>
        /// <param name="pageSize">Quantidade de posts por página (padrão 10).</param>
        /// <returns>Retorna uma coleção de <see cref="GetPostDto"/>.</returns>
        public async Task<IEnumerable<GetPostDto>> GetAllPosts(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Validate pagination parameters
                if (pageNumber < 1)
                    pageNumber = 1;
                    
                if (pageSize < 1 || pageSize > 100)
                    pageSize = 10;
                
                Log.Debug("Retrieving posts - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
                
                var posts = await _postRepository.GetAll(pageNumber, pageSize);
                var result = _mapper.Map<IEnumerable<GetPostDto>>(posts);
                
                Log.Debug("Retrieved {PostCount} posts", result.Count());
                
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving posts - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
                return Enumerable.Empty<GetPostDto>();
            }
        }

        /// <summary>
        /// Recupera um post pelo seu ID e o mapeia para o DTO de exibição.
        /// </summary>
        /// <param name="postId">ID do post a ser recuperado.</param>
        /// <returns>Retorna um <see cref="GetPostDto"/> ou null se o post não for encontrado.</returns>
        public async Task<GetPostDto?> GetPostById(long postId)
        {
            try
            {
                if (postId <= 0)
                {
                    Log.Warning("Invalid post ID requested: {PostId}", postId);
                    return null;
                }
                
                Log.Debug("Retrieving post {PostId}", postId);
                
                var post = await _postRepository.GetById(postId);
                if (post == null)
                {
                    Log.Debug("Post {PostId} not found", postId);
                    return null;
                }
                
                return _mapper.Map<GetPostDto>(post);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving post {PostId}", postId);
                return null;
            }
        }

        #region Helpers

        /// <summary>
        /// Cria a entidade <see cref="Post"/> a partir do DTO de criação.
        /// </summary>
        /// <param name="postDto">DTO contendo os dados para criação do post.</param>
        /// <returns>Retorna uma nova instância de <see cref="Post"/>.</returns>
        private Post CreatePostEntity(CreatePostDto postDto)
        {
            return new Post
            {
                Content = postDto.Content?.Trim(),
                UserId = postDto.UserId
            };
        }

        /// <summary>
        /// Processa o upload de novas imagens para um post recém-criado.
        /// </summary>
        /// <param name="post">A instância de <see cref="Post"/> criada.</param>
        /// <param name="images">Coleção de arquivos de imagem a serem enviados.</param>
        private async Task ProcessNewPostImagesAsync(Post post, IEnumerable<IFormFile>? images)
        {
            if (images != null && images.Any())
            {
                var postImages = new List<PostImage>();

                foreach (var image in images)
                {
                    var imageUrl = await _blobService.UploadFileAsync(image, "post-images");
                    postImages.Add(new PostImage
                    {
                        ImageUrl = imageUrl,
                        PostId = post.Id
                    });
                }

                await _postRepository.AddImages(postImages);
            }
        }

        /// <summary>
        /// Remove as imagens do post que não estão na lista de IDs a serem mantidas.
        /// </summary>
        /// <param name="post">O post a ser atualizado.</param>
        /// <param name="imagesToKeep">Coleção de IDs das imagens que devem ser mantidas.</param>
        private async Task RemoveImagesNotInKeepListAsync(Post post, IEnumerable<long>? imagesToKeep)
        {
            var keepIds = imagesToKeep ?? new List<long>();
            var imagesToRemove = post.Images.Where(img => !keepIds.Contains(img.Id)).ToList();

            foreach (var image in imagesToRemove)
            {
                await _blobService.DeleteFileAsync(image.ImageUrl);
                post.Images.Remove(image);
            }
        }

        /// <summary>
        /// Faz o upload e adiciona novas imagens ao post.
        /// </summary>
        /// <param name="post">O post a ser atualizado.</param>
        /// <param name="newImages">Coleção de novos arquivos de imagem a serem adicionados.</param>
        private async Task AddNewImagesAsync(Post post, IEnumerable<IFormFile>? newImages)
        {
            if (newImages != null && newImages.Any())
            {
                foreach (var file in newImages)
                {
                    var imageUrl = await _blobService.UploadFileAsync(file, "post-images");
                    var newImage = new PostImage
                    {
                        ImageUrl = imageUrl,
                        PostId = post.Id
                    };
                    post.Images.Add(newImage);
                }
            }
        }

        /// <summary>
        /// Deletes all images associated with a post from blob storage.
        /// </summary>
        /// <param name="post">The post whose images should be deleted.</param>
        private async Task DeletePostImagesAsync(Post post)
        {
            if (post.Images?.Any() == true)
            {
                var deleteTasks = post.Images.Select(img => _blobService.DeleteFileAsync(img.ImageUrl));
                await Task.WhenAll(deleteTasks);
                Log.Debug("Deleted {ImageCount} images for post {PostId}", post.Images.Count, post.Id);
            }
        }

        /// <summary>
        /// Validates the CreatePostDto input.
        /// </summary>
        /// <param name="postDto">The DTO to validate.</param>
        private void ValidateCreatePostDto(CreatePostDto postDto)
        {
            if (postDto == null)
                throw new ArgumentNullException(nameof(postDto));

            if (postDto.UserId <= 0)
                throw new ArgumentException("Valid user ID is required.");

            ValidatePostContent(postDto.Content);
        }

        /// <summary>
        /// Validates the UpdatePostDto input.
        /// </summary>
        /// <param name="postDto">The DTO to validate.</param>
        private void ValidateUpdatePostDto(UpdatePostDto postDto)
        {
            if (postDto == null)
                throw new ArgumentNullException(nameof(postDto));

            if (postDto.PostId <= 0)
                throw new ArgumentException("Valid post ID is required.");

            ValidatePostContent(postDto.Content);
        }

        /// <summary>
        /// Validates post content.
        /// </summary>
        /// <param name="content">The content to validate.</param>
        private void ValidatePostContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Post content cannot be empty.");

            if (content.Length > MaxContentLength)
                throw new ArgumentException($"Post content cannot exceed {MaxContentLength} characters.");

            // Basic content filtering (can be extended)
            if (ContainsSuspiciousContent(content))
                throw new InvalidOperationException("Post content contains prohibited content.");
        }

        /// <summary>
        /// Basic content filtering for suspicious patterns.
        /// </summary>
        /// <param name="content">Content to check.</param>
        /// <returns>True if content is suspicious.</returns>
        private bool ContainsSuspiciousContent(string content)
        {
            // Basic spam detection patterns (can be enhanced with more sophisticated filtering)
            var suspiciousPatterns = new[]
            {
                @"http[s]?://[^\s]+\.(tk|ml|ga|cf)", // Suspicious domains
                @"(.)\1{10,}", // Repeated characters
                @"[A-Z\s]{50,}" // Excessive caps
            };

            return suspiciousPatterns.Any(pattern => 
                Regex.IsMatch(content, pattern, RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// Validates uploaded images.
        /// </summary>
        /// <param name="images">Images to validate.</param>
        private async Task ValidateImagesAsync(IEnumerable<IFormFile>? images)
        {
            if (images == null) return;

            var imageList = images.ToList();
            
            if (imageList.Count > MaxImagesPerPost)
                throw new InvalidOperationException($"Cannot upload more than {MaxImagesPerPost} images per post.");

            foreach (var image in imageList)
            {
                await ValidateSingleImageAsync(image);
            }
        }

        /// <summary>
        /// Validates a single image file.
        /// </summary>
        /// <param name="image">Image to validate.</param>
        private async Task ValidateSingleImageAsync(IFormFile image)
        {
            if (image == null)
                throw new ArgumentException("Invalid image file.");

            if (image.Length == 0)
                throw new ArgumentException("Empty image file.");

            if (image.Length > MaxImageSizeBytes)
                throw new InvalidOperationException($"Image size cannot exceed {MaxImageSizeBytes / (1024 * 1024)}MB.");

            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!AllowedImageTypes.Contains(extension))
                throw new InvalidOperationException($"Unsupported image type. Allowed types: {string.Join(", ", AllowedImageTypes)}");

            // Validate image content type
            if (!image.ContentType.StartsWith("image/"))
                throw new InvalidOperationException("File must be an image.");

            // Additional validation: check if it's actually an image by reading header
            await ValidateImageHeaderAsync(image);
        }

        /// <summary>
        /// Validates image file by checking its header bytes.
        /// </summary>
        /// <param name="image">Image to validate.</param>
        private async Task ValidateImageHeaderAsync(IFormFile image)
        {
            var buffer = new byte[8];
            using var stream = image.OpenReadStream();
            await stream.ReadAsync(buffer, 0, 8);
            stream.Position = 0; // Reset for future reads

            var isValidImage = IsValidImageHeader(buffer, Path.GetExtension(image.FileName).ToLowerInvariant());
            if (!isValidImage)
                throw new InvalidOperationException("File does not appear to be a valid image.");
        }

        /// <summary>
        /// Checks if the file header matches the expected image format.
        /// </summary>
        /// <param name="header">File header bytes.</param>
        /// <param name="extension">File extension.</param>
        /// <returns>True if header matches expected format.</returns>
        private bool IsValidImageHeader(byte[] header, string extension)
        {
            return extension switch
            {
                ".jpg" or ".jpeg" => header[0] == 0xFF && header[1] == 0xD8,
                ".png" => header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47,
                ".gif" => (header[0] == 0x47 && header[1] == 0x49 && header[2] == 0x46),
                ".webp" => header[0] == 0x52 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x46,
                _ => true // Allow other types for now
            };
        }

        #endregion
    }
}
