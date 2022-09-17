using FDSService.Clients;
using FDSService.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace FDSService.EntityFrameworkCore.Clients;
public class ClientPackageRepository : EfCoreRepository<FDSServiceDbContext, ClientPackage, int>, IClientPackageRepository
{
    private readonly IPackageVersionRepository _packageVersionRepository;
    private readonly IClock _clock;
    private readonly ICurrentUser _currentUser;
    private readonly IIdentityUserRepository _identityUserRepository;

    public ClientPackageRepository(IDbContextProvider<FDSServiceDbContext> dbContextProvider, IPackageVersionRepository packageVersionRepository, IClock clock, ICurrentUser currentUser, IIdentityUserRepository identityUserRepository) : base(dbContextProvider)
    {
        _packageVersionRepository = packageVersionRepository;
        _clock = clock;
        _currentUser = currentUser;
        _identityUserRepository = identityUserRepository;
    }

    public virtual async Task<bool> CheckPackageIsExists(Guid clientId, Guid packageId, CancellationToken cancellationToken = default)
    {
        return await(await GetDbSetAsync()).AnyAsync(c => c.ClientId ==clientId && c.PackageId == packageId, GetCancellationToken(cancellationToken)).ConfigureAwait(false);
    }

    public virtual async Task<(List<ClientPackage>, long)> GetListAsync(string sorting, int skipCount, int maxResultCount, Guid clientId, CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync().ConfigureAwait(false);

        queryable = queryable
                            .AsNoTracking()
                            .Include(x => x.Package)
                            .Include(x => x.CurrentVersion)
                            .Where(x => x.ClientId == clientId);

        var result = queryable
                                .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(Package.CreationTime) : sorting)
                                .PageBy(skipCount, maxResultCount).ToList();

        var totalCount = queryable.LongCount();

        return (result, totalCount);
    }

    public async Task<List<PackageVersion>> GetPackageVersionsUpdatedListAsync(Guid packageId, CancellationToken cancellationToken = default)
    {
        var (countryId, channelId) = await GetUserDataAsync().ConfigureAwait(false);
        var queryableVersion= await _packageVersionRepository.GetQueryableAsync().ConfigureAwait(false);
                    var query =
                                from v in queryableVersion
                                join c in (await GetQueryableAsync().ConfigureAwait(false)) on v.PackageId equals c.PackageId
                                where 
                                        c.PackageId== packageId &&
                                        c.ClientId== _currentUser.GetId() &&
                                        v.AvailableDate <= _clock.Now.Date && 
                                        v.CreationTime> c.CurrentVersion.CreationTime &&
                                        v.Countries.Any(c => c.CountryId == countryId) &&
                                        v.Channels.Any(c => c.ChannelId == channelId)
                                orderby 
                                        c.CreationTime
                                select v;

        return await query.AsNoTracking().Include(a=> a.Attachment).ToListAsync(cancellationToken);
    }

    public async Task<PackageVersion> GetPackageVersionUpdatedAsync( Guid packageVersionId, CancellationToken cancellationToken = default)
    {
        var (countryId, channelId) = await GetUserDataAsync().ConfigureAwait(false);
        var packageversion = await _packageVersionRepository.GetQueryableAsync().ConfigureAwait(false);

        var query =
                    from v in packageversion
                    join c in (await GetQueryableAsync().ConfigureAwait(false)) on v.DependOnVersionId equals c.CurrentVersionId
                    where
                            v.Id == packageVersionId &&
                            c.ClientId == _currentUser.GetId() &&
                            v.AvailableDate <= _clock.Now.Date &&
                            v.CreationTime > c.CurrentVersion.CreationTime &&
                            v.Countries.Any(c => c.CountryId == countryId) &&
                            v.Channels.Any(c => c.ChannelId == channelId)
                    select v;



        return await query.AsNoTracking().FirstOrDefaultAsync();
    }

    private async Task<(int,int)> GetUserDataAsync()
    {
        var user = await _identityUserRepository.FindAsync(_currentUser.GetId()).ConfigureAwait(false);
        return (user.ExtraProperties["CountryId"].To<int>(), (int)user.ExtraProperties["ChannelId"].To<int>());
    }
}
