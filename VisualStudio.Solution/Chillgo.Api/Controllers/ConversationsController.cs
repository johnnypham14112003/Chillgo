using Chillgo.Api.Models.Request;
using Chillgo.BusinessService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chillgo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationsController : Controller
    {
        private readonly IConversationService _conversationService;

        public ConversationsController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConversations()
        {
            var conversations = await _conversationService.GetAllConversationsAsync();
            return Ok(conversations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConversationById(Guid id)
        {
            var conversation = await _conversationService.GetConversationByIdAsync(id);
            return Ok(conversation);
        }

        [HttpGet("by-account/{accountId}")]
        public async Task<IActionResult> GetConversationsByAccountId(Guid accountId)
        {
            var conversations = await _conversationService.GetConversationsByAccountIdAsync(accountId);
            return Ok(conversations);
        }

        [HttpGet("{conversationId}/messages")]
        public async Task<IActionResult> GetMessagesByConversationId(Guid conversationId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string orderBy = "sentTime", [FromQuery] string orderDirection = "desc")
        {
            var messages = await _conversationService.GetMessagesByConversationIdAsync(conversationId, page, pageSize, orderBy, orderDirection);
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateConversation([FromBody] CreateConversationRequest request)
        {
            var conversation = await _conversationService.CreateConversation(
                request.FirstAccountId,
                request.SecondAccountId,
                request.FirstName,
                request.SecondName,
                request.AIBotId
            );
            return Ok(conversation);
        }
    }
}
