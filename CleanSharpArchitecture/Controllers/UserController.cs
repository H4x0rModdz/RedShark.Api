using CleanSharpArchitecture.API.Attributes;
using CleanSharpArchitecture.API.Extensions;
using CleanSharpArchitecture.Application.DTOs.Users.Requests;
using CleanSharpArchitecture.Application.DTOs.Users.Responses;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Application.DTOs.UserPhotos.Requests;
using CleanSharpArchitecture.Application.DTOs.UserPhotos.Responses;
using CleanSharpArchitecture.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CleanSharpArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserPhotoService _userPhotoService;

        public UserController(IUserService userService, IUserPhotoService userPhotoService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userPhotoService = userPhotoService ?? throw new ArgumentNullException(nameof(userPhotoService));
        }

        /// <summary>
        /// Retrieves paginated list of users
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers(
            [FromQuery] EntityStatus? status,
            [FromQuery] string? include,
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Invalid pagination parameters.");

            var result = await _userService.GetAllUsers(status, include, pageNumber, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific user by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDto>> GetUserById(
            [Required] long id, 
            [FromQuery] EntityStatus? status, 
            [FromQuery] string? include)
        {
            if (id <= 0)
                return BadRequest("Invalid user ID.");

            var result = await _userService.GetUserById(id);
            if (result == null)
                return NotFound("User not found.");
                
            return Ok(result);
        }

        /// <summary>
        /// Retrieves user profile information by username
        /// </summary>
        [HttpGet("profile/{userName}")]
        [ProducesResponseType(typeof(UserProfileDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile([Required] string userName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return BadRequest("Invalid username.");

            var result = await _userService.GetUserProfile(userName, cancellationToken);
            if (result == null)
                return NotFound("User not found.");
                
            return Ok(result);
        }

        /// <summary>
        /// Updates user information
        /// </summary>
        [HttpPatch("{id}")]
        [RequireUserOwnership("id")]
        [ProducesResponseType(typeof(UpdateUserResultDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<UpdateUserResultDto>> UpdateUser(
            [Required] long id, 
            [FromForm, Required] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            updateUserDto.Id = id;
            var currentUserId = this.GetCurrentUserId();
            
            var result = await _userService.UpdateUser(updateUserDto, currentUserId);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a user account
        /// </summary>
        [HttpDelete("{id}")]
        [RequireUserOwnership("id")]
        [ProducesResponseType(typeof(DeleteUserResultDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<DeleteUserResultDto>> DeleteUser([Required] long id)
        {
            var currentUserId = this.GetCurrentUserId();
            var result = await _userService.DeleteUser(id, currentUserId);
            return Ok(result);
        }

        #region UserPhoto Endpoints

        /// <summary>
        /// Gets all photos for a specific user
        /// </summary>
        [HttpGet("{userId}/photos")]
        [ProducesResponseType(typeof(IEnumerable<UserPhotoDto>), 200)]
        public async Task<ActionResult<IEnumerable<UserPhotoDto>>> GetUserPhotos([Required] long userId)
        {
            var photos = await _userPhotoService.GetUserPhotos(userId);
            return Ok(photos);
        }

        /// <summary>
        /// Gets a specific photo by ID
        /// </summary>
        [HttpGet("photos/{photoId}")]
        [ProducesResponseType(typeof(UserPhotoDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserPhotoDto>> GetUserPhotoById([Required] long photoId)
        {
            var photo = await _userPhotoService.GetUserPhotoById(photoId);
            if (photo == null)
                return NotFound("Photo not found.");
            
            return Ok(photo);
        }

        /// <summary>
        /// Creates a new photo for the authenticated user
        /// </summary>
        [HttpPost("{userId}/photos")]
        [RequireUserOwnership("userId")]
        [ProducesResponseType(typeof(UserPhotoDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<UserPhotoDto>> CreateUserPhoto(
            [Required] long userId, 
            [FromBody] CreateUserPhotoDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var photo = await _userPhotoService.CreateUserPhoto(userId, request);
            return CreatedAtAction(nameof(GetUserPhotoById), new { photoId = photo.Id }, photo);
        }

        /// <summary>
        /// Updates a user photo
        /// </summary>
        [HttpPut("photos/{photoId}")]
        [RequireAuthentication]
        [ProducesResponseType(typeof(UserPhotoDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserPhotoDto>> UpdateUserPhoto(
            [Required] long photoId, 
            [FromBody] UpdateUserPhotoDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = this.GetCurrentUserId();
            
            try
            {
                var photo = await _userPhotoService.UpdateUserPhoto(photoId, currentUserId, request);
                return Ok(photo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a user photo
        /// </summary>
        [HttpDelete("photos/{photoId}")]
        [RequireAuthentication]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteUserPhoto([Required] long photoId)
        {
            var currentUserId = this.GetCurrentUserId();
            var success = await _userPhotoService.DeleteUserPhoto(photoId, currentUserId);
            
            if (!success)
                return NotFound("Photo not found or you don't have permission to delete it.");
            
            return Ok();
        }

        #endregion

    }
}


