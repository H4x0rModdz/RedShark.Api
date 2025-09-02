using CleanSharpArchitecture.Application.DTOs.Chats;
using CleanSharpArchitecture.Application.DTOs.Chats.ChatMessages;
using CleanSharpArchitecture.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CleanSharpArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<ActionResult<ChatDto>> CreateChat([FromBody] CreateChatRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var chat = await _chatService.CreateChat(request.Name, userId);
            return Ok(chat);
        }

        [HttpGet("{chatId}")]
        public async Task<ActionResult<ChatDto>> GetChatById(long chatId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var chat = await _chatService.GetChatById(chatId, userId);
            if (chat == null)
            {
                return NotFound();
            }

            return Ok(chat);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatDto>>> GetUserChats()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var chats = await _chatService.GetUserChats(userId);
            return Ok(chats);
        }

        [HttpPost("private")]
        public async Task<ActionResult<ChatDto>> GetOrCreatePrivateChat([FromBody] CreatePrivateChatRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var chat = await _chatService.GetOrCreatePrivateChat(userId, request.OtherUserId);
            return Ok(chat);
        }

        [HttpPost("{chatId}/messages")]
        public async Task<ActionResult<ChatMessageDto>> SendMessage(long chatId, [FromBody] SendMessageRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            try
            {
                var message = await _chatService.SendMessage(chatId, userId, request.Content);
                return Ok(message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpGet("{chatId}/messages")]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetChatMessages(
            long chatId, 
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 50)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            try
            {
                var messages = await _chatService.GetChatMessages(chatId, userId, pageNumber, pageSize);
                return Ok(messages);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPost("{chatId}/participants")]
        public async Task<ActionResult> AddUserToChat(long chatId, [FromBody] AddUserToChatRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var success = await _chatService.AddUserToChat(chatId, request.UserId, userId);
            if (!success)
            {
                return Forbid();
            }

            return Ok();
        }

        [HttpDelete("{chatId}/participants/{participantUserId}")]
        public async Task<ActionResult> RemoveUserFromChat(long chatId, long participantUserId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var success = await _chatService.RemoveUserFromChat(chatId, participantUserId, userId);
            if (!success)
            {
                return Forbid();
            }

            return Ok();
        }
    }

    public class CreateChatRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    public class CreatePrivateChatRequest
    {
        public long OtherUserId { get; set; }
    }

    public class SendMessageRequest
    {
        public string Content { get; set; } = string.Empty;
    }

    public class AddUserToChatRequest
    {
        public long UserId { get; set; }
    }
}