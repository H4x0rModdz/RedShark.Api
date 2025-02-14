using CleanSharpArchitecture.Application.DTOs.Users.Requests;
using CleanSharpArchitecture.Application.DTOs.Users.Responses;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSharpArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
            => _userService = userService;

        [HttpGet] // GET /api/user
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers(
            [FromQuery] EntityStatus? status,
            [FromQuery] string? include,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            return Ok(await _userService.GetAllUsers(status, include, pageNumber, pageSize));
        }

        [HttpGet("{id}")] // GET /api/user/{id}
        public async Task<ActionResult<UserDto>> GetUserById([FromQuery] long id, EntityStatus? status, string? include)
        {
            return await _userService.GetUserById(id);
        }

        [HttpPut("{id}")] // PUT /api/user/{id}
        public async Task<ActionResult<UpdateUserResultDto>> UpdateUser(long id, [FromBody] UpdateUserDto updateUserDto)
        {
            return await _userService.UpdateUser(updateUserDto);
        }

        [HttpDelete("{id}")] // DELETE /api/user/{id}
        public async Task<ActionResult<DeleteUserResultDto>> DeleteUser(long id)
        {
            return await _userService.DeleteUser(id);
        }
    }
}
