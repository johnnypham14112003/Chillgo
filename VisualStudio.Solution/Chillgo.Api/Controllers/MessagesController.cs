using Chillgo.Api.Models.Request;
using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chillgo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var message = new Message
            {
                Content = request.Content,
                SentTime = DateTime.UtcNow,
                ConversationId = request.ConversationId,
                SenderId = request.SenderId,
                Status = "Đã Gửi"
            };

            var createdMessage = await _messageService.CreateMessageAsync(message);
            return Ok(createdMessage);
        }

        [Authorize]
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateMessageStatus(Guid messageId, [FromBody] string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("Status không được để trống");
            }

            try
            {
                var updatedMessage = await _messageService.UpdateMessageStatusAsync(messageId, status);
                return Ok(updatedMessage);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Message không tìm thấy");
            }
        }

        [Authorize]
        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessageById(Guid messageId)
        {
            if (messageId == Guid.Empty)
            {
                return BadRequest("Message ID không hợp lệ");
            }

            try
            {
                await _messageService.DeleteMessageByIdAsync(messageId);
                return NoContent(); // 204 No Content on successful deletion
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Message không tìm thấy");
            }
        }
    }
}
