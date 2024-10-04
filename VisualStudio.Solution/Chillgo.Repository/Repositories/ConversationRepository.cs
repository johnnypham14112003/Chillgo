using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
