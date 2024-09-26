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
