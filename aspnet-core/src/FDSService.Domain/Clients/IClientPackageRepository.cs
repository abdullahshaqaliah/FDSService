using FDSService.Packages;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace FDSService.Clients;
public interface IClientPackageRepository : IRepository<ClientPackage, int>
{
    Task<(List<ClientPackage>, long)> GetListAsync(string sorting, int skipCount, int maxResultCount, Guid clientId, CancellationToken cancellationToken = default);
    Task<List<PackageVersion>> GetPackageVersionsUpdatedListAsync(Guid packageId, CancellationToken cancellationToken = default);
    Task<PackageVersion> GetPackageVersionUpdatedAsync(Guid packageVersionId, CancellationToken cancellationToken = default);
    Task<bool> CheckPackageIsExists(Guid clientId,Guid packageId, CancellationToken cancellationToken = default);
}