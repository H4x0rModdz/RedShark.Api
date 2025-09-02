using AutoMapper;
using CleanSharpArchitecture.Application.DTOs.Auth.Request;
using CleanSharpArchitecture.Application.DTOs.Users.Requests;
using CleanSharpArchitecture.Application.DTOs.Users.Responses;
using CleanSharpArchitecture.Application.DTOs.Posts.Request;
using CleanSharpArchitecture.Application.DTOs.Badges.UserBadges;
using CleanSharpArchitecture.Application.DTOs.Followers;
using CleanSharpArchitecture.Domain.Interfaces;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Application.DTOs.UserPhotos.Requests;
using CleanSharpArchitecture.Application.DTOs.UserPhotos.Responses;
using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;
using CleanSharpArchitecture.Domain.ValueObjects;
using CleanSharpArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace CleanSharpArchitecture.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;
        private readonly IUserPhotoService _userPhotoService;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IBlobService blobService,
            IUserPhotoService userPhotoService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _blobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
            _userPhotoService = userPhotoService ?? throw new ArgumentNullException(nameof(userPhotoService));
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers(EntityStatus? status = null, string? include = null, int pageNumber = 1, int pageSize = 10)
        {
            var includes = include?.Split(',') ?? Array.Empty<string>();
            var users = await _userRepository.GetAllUsers(status, include, pageNumber, pageSize);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserById(long id)
        {
            var user = await FindUser(id);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task<UserProfileDto?> GetUserProfile(string userName, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.SelectByUserName(userName);
            if (user == null)
                return null;
            
            return _mapper.Map<UserProfileDto>(user);
        }

        public async Task<UpdateUserResultDto> UpdateUser(UpdateUserDto updateUserDto, long currentUserId)
        {
            try
            {
                var user = await FindUser(updateUserDto.Id);
                UpdateUserFields(user, updateUserDto);

                if (updateUserDto.ProfileImage is not null)
                {
                    var imageUrl = await UploadProfileImageAsync(updateUserDto.ProfileImage);
                    user.ProfileImageUrl = imageUrl;
                }

                if (updateUserDto.CoverImage is not null)
                {
                    var imageUrl = await UploadCoverImageAsync(updateUserDto.CoverImage);
                    user.CoverImageUrl = imageUrl;
                }

                user.UpdatedAt = DateTime.UtcNow;
                await _userRepository.Update(user);

                Log.Information($"Usuário {user.Name} atualizado com sucesso.");
                return new UpdateUserResultDto { Success = true, Errors = new List<string>() };
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao atualizar usuário: {ex.Message}");
                return new UpdateUserResultDto { Success = false, Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<DeleteUserResultDto> DeleteUser(long id, long currentUserId)
        {
            try
            {
                var user = await FindUser(id);
                user.Status = EntityStatus.Deleted;
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.Update(user);
                Log.Information("User {UserName} deleted successfully", user.Name);

                return new DeleteUserResultDto { Success = true, Errors = new List<string>() };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting user");
                return new DeleteUserResultDto { Success = false, Errors = new List<string> { "An error occurred while deleting the user." } };
            }
        }

        private async Task<User> FindUser(long id)
        {
            var user = await _userRepository.SelectById(id);
            if (user is null)
                throw new Exception("Usuário não encontrado.");

            return user;
        }

        private void UpdateUserFields(User user, UpdateUserDto updateUserDto)
        {
            if (!string.IsNullOrWhiteSpace(updateUserDto.Name))
                user.Name = updateUserDto.Name;
            if (!string.IsNullOrWhiteSpace(updateUserDto.Email))
                user.Email = Email.Create(updateUserDto.Email);
            if (!string.IsNullOrWhiteSpace(updateUserDto.Biography))
                user.Biography = updateUserDto.Biography;
            if (!string.IsNullOrWhiteSpace(updateUserDto.Location))
                user.Location = updateUserDto.Location;
            if (!string.IsNullOrWhiteSpace(updateUserDto.Website))
                user.Website = updateUserDto.Website;
            if (!string.IsNullOrWhiteSpace(updateUserDto.Password))
                user.Password = updateUserDto.Password;
            if (updateUserDto.Status is not null)
                user.Status = updateUserDto.Status.Value;
        }

        private async Task<string> UploadProfileImageAsync(IFormFile profileImage)
        {
            return await _blobService.UploadFileAsync(profileImage, "user-profile");
        }

        private async Task<string> UploadCoverImageAsync(IFormFile coverImage)
        {
            return await _blobService.UploadFileAsync(coverImage, "user-cover");
        }

        #region UserPhoto Methods

        public async Task<IEnumerable<UserPhoto>> GetUserPhotos(long userId)
        {
            var photoDtos = await _userPhotoService.GetUserPhotos(userId);
            return photoDtos.Select(dto => new UserPhoto 
            { 
                Id = dto.Id, 
                UserId = dto.UserId, 
                ImageUrl = dto.ImageUrl, 
                Description = dto.Description,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            });
        }

        public async Task<UserPhoto?> GetUserPhotoById(long photoId)
        {
            var photoDto = await _userPhotoService.GetUserPhotoById(photoId);
            if (photoDto == null) return null;
            
            return new UserPhoto 
            { 
                Id = photoDto.Id, 
                UserId = photoDto.UserId, 
                ImageUrl = photoDto.ImageUrl, 
                Description = photoDto.Description,
                CreatedAt = photoDto.CreatedAt,
                UpdatedAt = photoDto.UpdatedAt
            };
        }

        public async Task<UserPhoto> CreateUserPhoto(long userId, string imageUrl, string? description = null)
        {
            var createDto = new CreateUserPhotoDto 
            { 
                ImageUrl = imageUrl, 
                Description = description 
            };
            
            var photoDto = await _userPhotoService.CreateUserPhoto(userId, createDto);
            return new UserPhoto 
            { 
                Id = photoDto.Id, 
                UserId = photoDto.UserId, 
                ImageUrl = photoDto.ImageUrl, 
                Description = photoDto.Description,
                CreatedAt = photoDto.CreatedAt,
                UpdatedAt = photoDto.UpdatedAt
            };
        }

        public async Task<UserPhoto> UpdateUserPhoto(long photoId, long userId, string? imageUrl = null, string? description = null)
        {
            var updateDto = new UpdateUserPhotoDto 
            { 
                ImageUrl = imageUrl, 
                Description = description 
            };
            
            var photoDto = await _userPhotoService.UpdateUserPhoto(photoId, userId, updateDto);
            return new UserPhoto 
            { 
                Id = photoDto.Id, 
                UserId = photoDto.UserId, 
                ImageUrl = photoDto.ImageUrl, 
                Description = photoDto.Description,
                CreatedAt = photoDto.CreatedAt,
                UpdatedAt = photoDto.UpdatedAt
            };
        }

        public async Task<bool> DeleteUserPhoto(long photoId, long userId)
        {
            return await _userPhotoService.DeleteUserPhoto(photoId, userId);
        }

        #endregion

    }
}
