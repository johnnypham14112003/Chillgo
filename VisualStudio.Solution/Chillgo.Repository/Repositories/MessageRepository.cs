using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;

namespace Chillgo.Repository.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(ChillgoDbContext context) : base(context)
        {
        }
    }
}
