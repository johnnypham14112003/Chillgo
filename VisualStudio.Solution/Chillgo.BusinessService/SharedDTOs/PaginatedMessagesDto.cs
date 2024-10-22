using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chillgo.BusinessService.SharedDTOs
{
    public class PaginatedMessagesDto
    {
        public Guid ConversationId { get; set; }
        public List<MessageDto> Messages { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
