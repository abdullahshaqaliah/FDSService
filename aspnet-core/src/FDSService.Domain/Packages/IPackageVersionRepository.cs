using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace FDSService.Packages;
public interface IPackageVersionRepository : IRepository<PackageVersion, Guid>
{
    Task<(List<PackageVersion>, long)> GetListAsync(string sorting, int skipCount, int maxResultCount, string filter, Guid? packageId,int? countryId,int? channelId, PackageVersionType? type, CancellationToken cancellationToken = default);

    Task<bool> CheckNameOrNumberAsync(string name,int VersionNumber, Guid packageId, Guid? expectedId = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Check if the package has default version or not
    /// </summary>
    /// <param name="packageId">package id</param>
    /// <returns>bool value</returns>
    Task<bool> CheckHasDefaultVersionAsync(Guid packageId, Guid? expectedId = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Check if the version dependency is related to another version
    /// </summary>
    /// <param name="id">Version package id</param>
    /// <param name="packageId">package id</param>
    /// <returns>bool value</returns>

    Task<bool> CheckIfVersionDependencyIsRelatedWithAnotherVersionAsync(Guid id, Guid? expectedId = null, CancellationToken cancellationToken = default);


}
