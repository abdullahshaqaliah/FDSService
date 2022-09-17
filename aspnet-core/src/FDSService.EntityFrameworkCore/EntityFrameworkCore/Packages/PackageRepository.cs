using FDSService.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace FDSService.EntityFrameworkCore.Packages;
public class PackageRepository : EfCoreRepository<FDSServiceDbContext, Package, Guid>, IPackageRepository
{
    public PackageRepository(IDbContextProvider<FDSServiceDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }


    public virtual async Task<(List<Package>, long)> GetListAsync(string sorting, int skipCount, int maxResultCount, string filter, bool? isActive, CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync().ConfigureAwait(false);

        queryable = queryable
                            .AsNoTracking()
                            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter))
                            .WhereIf(isActive.HasValue, x => x.IsActive == isActive);

        var result = queryable
                                .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(Package.CreationTime) : sorting)
                                .PageBy(skipCount, maxResultCount).ToList();

        var totalCount = queryable.LongCount();

        return (result, totalCount);
    }
    public virtual async Task<bool> CheckNameAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(c => c.Id != expectedId && c.Name == name, GetCancellationToken(cancellationToken)).ConfigureAwait(false);
    }

}