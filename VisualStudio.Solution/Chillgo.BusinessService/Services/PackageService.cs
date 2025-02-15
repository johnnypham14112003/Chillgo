﻿using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.Extensions.Exceptions;
using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Interfaces;
using Mapster;

namespace Chillgo.BusinessService.Services
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BM_PagingResults<BM_PackageInfo>> GetListPackage(BM_PackageQuery queryCondition)
        {
            var (result, totalCount) = await _unitOfWork.GetPackageRepository().GetPackagesListAsync
                (queryCondition.KeyWord, queryCondition.Price, queryCondition.Duration, queryCondition.Status);

            if (totalCount == 0) { throw new NotFoundException("Not found any package"); }

            // Convert to return data type
            var mappedResult = result.Adapt<List<BM_PackageInfo>>();

            return new BM_PagingResults<BM_PackageInfo>
            {
                TotalCount = totalCount,
                DataList = mappedResult
            };
        }
    }
}
