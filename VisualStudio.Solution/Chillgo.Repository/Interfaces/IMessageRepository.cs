using Chillgo.Repository.Models;

namespace Chillgo.Repository.Interfaces
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<Message> GetByIdAsync(Guid messageId);
    }
}
