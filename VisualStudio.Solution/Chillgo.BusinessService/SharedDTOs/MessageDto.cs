using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chillgo.BusinessService.SharedDTOs
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime SentTime { get; set; }
        public Guid SenderId { get; set; }
        public string SenderName { get; set; }
        public Guid? BotReplyId { get; set; }
    }
}
