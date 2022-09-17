using FDSService.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace FDSService.EntityFrameworkCore.Packages;
public class PackageVersionRepository : EfCoreRepository<FDSServiceDbContext, PackageVersion, Guid>, IPackageVersionRepository
{
    public PackageVersionRepository(IDbContextProvider<FDSServiceDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
    public virtual async Task<(List<PackageVersion>, long)> GetListAsync(string sorting, int skipCount, int maxResultCount, string filter, Guid? packageId, int? countryId, int? channelId, PackageVersionType? type, CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync().ConfigureAwait(false);


        queryable = queryable
                            .AsNoTracking()
                            .Include(p=> p.Package)
                            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter))
                            .WhereIf(packageId.HasValue, x => x.PackageId == packageId.Value)
                            .WhereIf(countryId.HasValue, x => x.Countries.Any(c=> c.CountryId== countryId.Value))
                            .WhereIf(channelId.HasValue, x => x.Channels.Any(c => c.ChannelId == channelId.Value))
                            .WhereIf(type.HasValue, x => x.Type == type.Value);
        var result = queryable
                                .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(Package.CreationTime) : sorting)
                                .PageBy(skipCount, maxResultCount).ToList();

        var totalCount = queryable.LongCount();

        return (result, totalCount);
    }
    public virtual async Task<bool> CheckNameOrNumberAsync(string name, int VersionNumber, Guid packageId, Guid? expectedId = null, CancellationToken cancellationToken = default)
    {
        return await(await GetDbSetAsync()).AnyAsync(c => c.Id != expectedId && (c.Name == name || c.VersionNumber== VersionNumber) && c.PackageId== packageId, GetCancellationToken(cancellationToken)).ConfigureAwait(false);
    }

    public virtual async Task<bool> CheckHasDefaultVersionAsync(Guid packageId, Guid? expectedId = null, CancellationToken cancellationToken = default)
    {
        return await(await GetDbSetAsync()).AnyAsync(v => v.PackageId == packageId && v.Id != expectedId && !v.DependOnVersionId.HasValue, GetCancellationToken(cancellationToken)).ConfigureAwait(false);

    }

    public virtual async Task<bool> CheckIfVersionDependencyIsRelatedWithAnotherVersionAsync(Guid id, Guid? expectedId = null,CancellationToken cancellationToken = default)
    {
        return await(await GetDbSetAsync()).AnyAsync(v => v.DependOnVersionId== id && v.Id != expectedId, GetCancellationToken(cancellationToken)).ConfigureAwait(false);
    }
    public override async Task<IQueryable<PackageVersion>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}