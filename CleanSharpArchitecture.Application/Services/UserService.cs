using AutoMapper;
using CleanSharpArchitecture.Application.DTOs.Auth.Request;
using CleanSharpArchitecture.Application.DTOs.Users.Requests;
using CleanSharpArchitecture.Application.DTOs.Users.Responses;
using CleanSharpArchitecture.Application.Repositories.Interfaces;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Enums;
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

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IBlobService blobService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers(EntityStatus? status = null, string? include = null, int pageNumber = 1, int pageSize = 10)
        {
            var includes = include?.Split(',') ?? Array.Empty<string>();
            var users = await _userRepository.GetAllUsers(status, include, pageNumber, pageSize);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserById(Guid id)
        {
            var user = await FindUser(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UpdateUserResultDto> UpdateUser(UpdateUserDto updateUserDto)
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

                user.UpdatedAt = DateTime.Now;
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

        public async Task<DeleteUserResultDto> DeleteUser(Guid id)
        {
            try
            {
                var user = await FindUser(id);
                user.Status = EntityStatus.Deleted;
                user.UpdatedAt = DateTime.Now;

                await _userRepository.Update(user);
                Log.Information($"Usuário {user.Name} removido com sucesso.");

                return new DeleteUserResultDto { Success = true, Errors = new List<string>() };
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao remover usuário: {ex.Message}");
                return new DeleteUserResultDto { Success = false, Errors = new List<string> { ex.Message } };
            }
        }

        private async Task<User> FindUser(Guid id)
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
                user.Email = updateUserDto.Email;
            if (!string.IsNullOrWhiteSpace(updateUserDto.Biography))
                user.Biography = updateUserDto.Biography;
            if (!string.IsNullOrWhiteSpace(updateUserDto.Password))
                user.Password = updateUserDto.Password;
            if (updateUserDto.Status is not null)
                user.Status = updateUserDto.Status.Value;
        }

        private async Task<string> UploadProfileImageAsync(IFormFile profileImage)
        {
            return await _blobService.UploadFileAsync(profileImage, "user-profile");
        }
    }
}
