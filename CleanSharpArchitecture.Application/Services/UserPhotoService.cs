using AutoMapper;
using CleanSharpArchitecture.Application.DTOs.UserPhotos.Requests;
using CleanSharpArchitecture.Application.DTOs.UserPhotos.Responses;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Interfaces;
using Serilog;

namespace CleanSharpArchitecture.Application.Services
{
    public class UserPhotoService : IUserPhotoService
    {
        private readonly IUserPhotoRepository _userPhotoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserPhotoService(
            IUserPhotoRepository userPhotoRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userPhotoRepository = userPhotoRepository ?? throw new ArgumentNullException(nameof(userPhotoRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<UserPhotoDto>> GetUserPhotos(long userId)
        {
            var photos = await _userPhotoRepository.GetByUserId(userId);
            return _mapper.Map<IEnumerable<UserPhotoDto>>(photos);
        }

        public async Task<UserPhotoDto?> GetUserPhotoById(long photoId)
        {
            var photo = await _userPhotoRepository.GetById(photoId);
            return photo != null ? _mapper.Map<UserPhotoDto>(photo) : null;
        }

        public async Task<UserPhotoDto> CreateUserPhoto(long userId, CreateUserPhotoDto createDto)
        {
            // Verify user exists
            var user = await _userRepository.SelectById(userId);
            if (user == null)
                throw new Exception("Usuário não encontrado.");

            var photo = new UserPhoto
            {
                UserId = userId,
                ImageUrl = createDto.ImageUrl,
                Description = createDto.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdPhoto = await _userPhotoRepository.Create(photo);
            Log.Information("Foto criada para usuário {UserId}: {PhotoId}", userId, createdPhoto.Id);
            
            return _mapper.Map<UserPhotoDto>(createdPhoto);
        }

        public async Task<UserPhotoDto> UpdateUserPhoto(long photoId, long userId, UpdateUserPhotoDto updateDto)
        {
            var photo = await _userPhotoRepository.GetById(photoId);
            if (photo == null || photo.UserId != userId)
                throw new Exception("Foto não encontrada ou você não tem permissão para editá-la.");

            if (!string.IsNullOrWhiteSpace(updateDto.ImageUrl))
                photo.ImageUrl = updateDto.ImageUrl;
            
            if (updateDto.Description != null)
                photo.Description = updateDto.Description;

            photo.UpdatedAt = DateTime.UtcNow;

            await _userPhotoRepository.Update(photo);
            Log.Information("Foto {PhotoId} atualizada pelo usuário {UserId}", photoId, userId);
            
            return _mapper.Map<UserPhotoDto>(photo);
        }

        public async Task<bool> DeleteUserPhoto(long photoId, long userId)
        {
            var photo = await _userPhotoRepository.GetById(photoId);
            if (photo == null || photo.UserId != userId)
                return false;

            await _userPhotoRepository.Delete(photoId);
            Log.Information("Foto {PhotoId} deletada pelo usuário {UserId}", photoId, userId);
            
            return true;
        }
    }
}