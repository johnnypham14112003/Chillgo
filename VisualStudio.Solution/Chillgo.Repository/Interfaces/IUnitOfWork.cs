namespace Chillgo.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        IAccountRepository GetAccountRepository();
        ILocationRepository GetLocationRepository();
        IConversationRepository GetConversationRepository();
        IMessageRepository GetMessageRepository();
        IPackageTransactionRepository GetPackageTransactionRepository();
        IImageRepository GetImageRepository();
    }
}
