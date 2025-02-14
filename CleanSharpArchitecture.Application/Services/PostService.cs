using AutoMapper;
using CleanSharpArchitecture.Application.DTOs.Posts.Request;
using CleanSharpArchitecture.Application.DTOs.Posts.Response;
using CleanSharpArchitecture.Application.Interfaces.Repositories;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Domain.Entities.Posts;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace CleanSharpArchitecture.Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly BlobService _blobService;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository, BlobService blobService, IMapper mapper)
        {
            _postRepository = postRepository;
            _blobService = blobService;
            _mapper = mapper;
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
                var post = CreatePostEntity(postDto);
                var createdPost = await _postRepository.Create(post);

                await ProcessNewPostImages(createdPost, postDto.Images);

                Log.Information($"Post {createdPost.Id} created successfully.");

                return new PostResultDto
                {
                    Success = true,
                    PostId = createdPost.Id.ToString(),
                    Errors = new List<string>()
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Error creating post: {ex.Message}");
                return new PostResultDto
                {
                    Success = false,
                    Errors = new List<string> { ex.Message }
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
                var post = await _postRepository.GetById(postDto.PostId)
                           ?? throw new Exception("Post not found.");

                post.Content = postDto.Content;

                await RemoveImagesNotInKeepList(post, postDto.ImagesToKeep);
                await AddNewImages(post, postDto.NewImages);

                await _postRepository.Update(post);

                Log.Information($"Post {post.Id} updated successfully.");

                return new PostResultDto
                {
                    Success = true,
                    PostId = post.Id.ToString(),
                    Errors = new List<string>()
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating post: {ex.Message}");
                return new PostResultDto
                {
                    Success = false,
                    Errors = new List<string> { ex.Message }
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
                var post = await _postRepository.GetById(postId)
                           ?? throw new Exception("Post not found.");

                await _postRepository.Delete(postId);

                Log.Information($"Post {postId} deleted successfully.");

                return new PostResultDto { Success = true, Errors = new List<string>() };
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting post: {ex.Message}");
                return new PostResultDto { Success = false, Errors = new List<string> { ex.Message } };
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
            var posts = await _postRepository.GetAll(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<GetPostDto>>(posts);
        }

        /// <summary>
        /// Recupera um post pelo seu ID e o mapeia para o DTO de exibição.
        /// </summary>
        /// <param name="postId">ID do post a ser recuperado.</param>
        /// <returns>Retorna um <see cref="GetPostDto"/> ou null se o post não for encontrado.</returns>
        public async Task<GetPostDto?> GetPostById(long postId)
        {
            var post = await _postRepository.GetById(postId);
            return post is null ? null : _mapper.Map<GetPostDto>(post);
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
                Content = postDto.Content,
                UserId = postDto.UserId
            };
        }

        /// <summary>
        /// Processa o upload de novas imagens para um post recém-criado.
        /// </summary>
        /// <param name="post">A instância de <see cref="Post"/> criada.</param>
        /// <param name="images">Coleção de arquivos de imagem a serem enviados.</param>
        private async Task ProcessNewPostImages(Post post, IEnumerable<IFormFile>? images)
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
        private async Task RemoveImagesNotInKeepList(Post post, IEnumerable<long>? imagesToKeep)
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
        private async Task AddNewImages(Post post, IEnumerable<IFormFile>? newImages)
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

        #endregion
    }
}
