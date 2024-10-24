using Chillgo.Api.Models.Request;
using Chillgo.BusinessService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chillgo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationsController : Controller
    {
        private readonly IServiceFactory _serviceFactory;
        public ConversationsController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllConversations()
        {
            var conversations = await _serviceFactory.GetConversationService().GetAllConversationsAsync();
            return Ok(conversations);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConversationById(Guid id)
        {
            var conversation = await _serviceFactory.GetConversationService().GetConversationByIdAsync(id);
            return Ok(conversation);
        }

        [Authorize]
        [HttpGet("by-account/{accountId}")]
        public async Task<IActionResult> GetConversationsByAccountId(Guid accountId)
        {
            var conversations = await _serviceFactory.GetConversationService().GetConversationsByAccountIdAsync(accountId);
            return Ok(conversations);
        }

        [Authorize]
        [HttpGet("{conversationId}/messages")]
        public async Task<IActionResult> GetMessagesByConversationId(Guid conversationId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string orderBy = "sentTime", [FromQuery] string orderDirection = "desc")
        {
            var messages = await _serviceFactory.GetConversationService().GetMessagesByConversationIdAsync(conversationId, page, pageSize, orderBy, orderDirection);
            return Ok(messages);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateConversation([FromBody] CreateConversationRequest request)
        {
            var conversation = await _serviceFactory.GetConversationService().CreateConversation(
                request.FirstAccountId,
                request.SecondAccountId,
                request.FirstName,
                request.SecondName,
                request.AIBotId
            );
            return Ok(conversation);
        }

        [Authorize]
        [HttpDelete("{conversationId}")]
        public async Task<IActionResult> DeleteConversationById(Guid conversationId)
        {
            if (conversationId == Guid.Empty)
            {
                return BadRequest("Conversation ID không hợp lệ");
            }

            try
            {
                await _serviceFactory.GetConversationService().DeleteConversationByIdAsync(conversationId);
                return NoContent(); // 204 No Content on successful deletion
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Conversation không tìm thấy");
            }
        }
    }
}
