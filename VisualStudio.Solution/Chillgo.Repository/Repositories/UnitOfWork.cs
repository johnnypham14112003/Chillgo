using Chillgo.Repository.Interfaces;

namespace Chillgo.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChillgoDbContext _context;
        //==========================================================

        private readonly Lazy<IAccountRepository> _accountRepository;
        private readonly Lazy<ILocationRepository> _locationRepository;
        private readonly Lazy<IConversationRepository> _conversationRepository;
        private readonly Lazy<IMessageRepository> _messageRepository;
        private readonly Lazy<IPackageTransactionRepository> _packageTransactionRepository;
        private readonly Lazy<IImageRepository> _imageRepository;

        public UnitOfWork(ChillgoDbContext context)
        {
            _context = context;
            //======================================================================================

            _accountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(context));
            _locationRepository = new Lazy<ILocationRepository>(() => new LocationRepository(context));
            _conversationRepository = new Lazy<IConversationRepository>(() => new ConversationRepository(context));
            _messageRepository = new Lazy<IMessageRepository>(() => new MessageRepository(context));
            _packageTransactionRepository = new Lazy<IPackageTransactionRepository>(() => new PackageTransactionRepository(context));
            _imageRepository = new Lazy<IImageRepository>(() => new ImageRepository(context));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IAccountRepository GetAccountRepository()
        {
            return _accountRepository.Value;
        }

        public ILocationRepository GetLocationRepository()
        {
            return _locationRepository.Value;
        }

        public IConversationRepository GetConversationRepository()
        {
            return _conversationRepository.Value;
        }

        public IMessageRepository GetMessageRepository()
        {
            return _messageRepository.Value;
        }
        public IPackageTransactionRepository GetPackageTransactionRepository()
        {
            return _packageTransactionRepository.Value;
        }

        public IImageRepository GetImageRepository()
        {
            return _imageRepository.Value;
        }

    }
}
