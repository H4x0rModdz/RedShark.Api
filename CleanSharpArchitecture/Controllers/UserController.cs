using CleanSharpArchitecture.Application.DTOs.Users.Requests;
using CleanSharpArchitecture.Application.DTOs.Users.Responses;
using CleanSharpArchitecture.Application.Services.Interfaces;
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

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
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
        /// Updates user information
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateUserResultDto), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UpdateUserResultDto>> UpdateUser(
            [Required] long id, 
            [FromBody, Required] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id <= 0)
                return BadRequest("Invalid user ID.");

            var result = await _userService.UpdateUser(updateUserDto);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a user account
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteUserResultDto), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DeleteUserResultDto>> DeleteUser([Required] long id)
        {
            if (id <= 0)
                return BadRequest("Invalid user ID.");

            var result = await _userService.DeleteUser(id);
            return Ok(result);
        }
    }
}
