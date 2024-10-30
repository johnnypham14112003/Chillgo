using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;

namespace Chillgo.Repository.Repositories
{
    public class ConversationRepository : GenericRepository<Conversation>, IConversationRepository
    {
        private readonly ChillgoDbContext _context;
        public ConversationRepository(ChillgoDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
