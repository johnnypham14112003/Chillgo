using Chillgo.BusinessService.BusinessModels;

namespace Chillgo.BusinessService.Interfaces
{
    public interface IPackageService
    {
        Task<BM_PagingResults<BM_PackageInfo>> GetListPackage(BM_PackageQuery queryCondition);
    }
}
