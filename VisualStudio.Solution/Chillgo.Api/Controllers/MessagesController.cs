using Chillgo.Api.Models.Request;
using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Models;
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
    }
}
