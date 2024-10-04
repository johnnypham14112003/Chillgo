using Chillgo.Repository.Interfaces;

namespace Chillgo.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChillgoDbContext _context;
        //==========================================================

        private readonly Lazy<IAccountRepository> _accountRepository;
        private readonly Lazy<IConversationRepository> _conversationRepository;
        private readonly Lazy<IMessageRepository> _messageRepository;

        public UnitOfWork(ChillgoDbContext context)
        {
            _context = context;
            //======================================================================================

            _accountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(context));
            _conversationRepository = new Lazy<IConversationRepository>(() => new ConversationRepository(context));
            _messageRepository = new Lazy<IMessageRepository>(() => new MessageRepository(context));
        }

        public IConversationRepository ConversationRepository => _conversationRepository.Value;
        public IMessageRepository MessageRepository => _messageRepository.Value;

        public IAccountRepository GetAccountRepository()
        {
            return _accountRepository.Value;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
