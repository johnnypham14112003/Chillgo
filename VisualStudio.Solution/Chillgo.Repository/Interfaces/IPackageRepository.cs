using Chillgo.Repository.Models;

namespace Chillgo.Repository.Interfaces
{
    public interface IPackageRepository : IGenericRepository<Package>
    {
        Task<(List<Package> result, int totalCount)> GetPackagesListAsync
            (string? keyword, decimal? price, short? duration, string? Status);
    }
}
