namespace Chillgo.BusinessService.Interfaces
{
    public interface IServiceFactory
    {
        IAccountService GetAccountService();
        ILocationService GetLocationService();
        IConversationService GetConversationService();
        IMessageService GetMessageService();
        IFirebaseStorageService GetFirebaseStorageService();
    }
}
