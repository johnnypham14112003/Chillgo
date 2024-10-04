using Chillgo.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chillgo.BusinessService.Interfaces
{
    public interface IMessageService
    {
        Task<Message> CreateMessageAsync(Message message);
        Task<Message> UpdateMessageStatusAsync(Guid messageId, string status);
    }
}
