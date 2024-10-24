using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using AutoMapper;

namespace Chillgo.BusinessService.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly Lazy<IAccountService> _accountService;
        private readonly Lazy<IConversationService> _conversationService;
        private readonly Lazy<ILocationService> _locationService;
        private readonly Lazy<IMessageService> _messageService;
        private readonly Lazy<IFirebaseStorageService> _firebaseStorageService;

        public ServiceFactory(
            IUnitOfWork unitOfWork,
            IAuthenticationService authenticationService,
            string bucketName,
            IMapper mapper)
        {
            _accountService = new Lazy<IAccountService>(() => new AccountService(unitOfWork, authenticationService));
            _conversationService = new Lazy<IConversationService>(() => new ConversationService(unitOfWork, mapper));
            _locationService = new Lazy<ILocationService>(() => new LocationService(unitOfWork));
            _messageService = new Lazy<IMessageService>(() => new MessageService(unitOfWork));

            _firebaseStorageService = new Lazy<IFirebaseStorageService>(() => new FirebaseStorageService(unitOfWork, bucketName));
        }

        public IAccountService GetAccountService() => _accountService.Value;
        public ILocationService GetLocationService() => _locationService.Value;
        public IConversationService GetConversationService() => _conversationService.Value;
        public IMessageService GetMessageService() => _messageService.Value;
        public IFirebaseStorageService GetFirebaseStorageService() => _firebaseStorageService.Value;
    }
}
