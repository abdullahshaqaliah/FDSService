using FDSService.Packages;
using System.Linq;

namespace FDSService.EntityFrameworkCore;
public static class FDSServiceEfCoreQueryableExtensions
{
    public static IQueryable<PackageVersion> IncludeDetails(this IQueryable<PackageVersion> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }
        return queryable
                .Include(s=> s.Channels)
                .Include(c=> c.Countries);

    }
}
