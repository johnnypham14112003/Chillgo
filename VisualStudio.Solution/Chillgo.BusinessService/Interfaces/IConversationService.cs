using Chillgo.BusinessService.SharedDTOs;
using Chillgo.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chillgo.BusinessService.Interfaces
{
    public interface IConversationService
    {
        Task<Conversation> CreateConversation(Guid firstAccountId, Guid secondAccountId, string firstName, string secondName, Guid? aiBotId = null);

        Task<List<ConversationDto>> GetAllConversationsAsync();

        Task<ConversationDto> GetConversationByIdAsync(Guid conversationId);

        Task<List<ConversationDto>> GetConversationsByAccountIdAsync(Guid accountId);

        Task<PaginatedMessagesDto> GetMessagesByConversationIdAsync(Guid conversationId, int page, int pageSize, string orderBy, string orderDirection);

        Task DeleteConversationByIdAsync(Guid conversationId);
    }
}
