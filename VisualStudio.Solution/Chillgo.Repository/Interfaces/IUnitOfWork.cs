﻿namespace Chillgo.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        IAccountRepository GetAccountRepository();
        ILocationRepository GetLocationRepository();
        IConversationRepository GetConversationRepository();
        IMessageRepository GetMessageRepository();
        IPackageRepository GetPackageRepository();
        IPackageTransactionRepository GetPackageTransactionRepository();
        IImageRepository GetImageRepository();
    }
}
