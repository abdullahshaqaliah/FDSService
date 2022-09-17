using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace FDSService.Packages;
public interface IPackageRepository: IRepository<Package, Guid>
{
    Task<(List<Package>, long)> GetListAsync(string sorting, int skipCount, int maxResultCount, string filter, bool? isActive, CancellationToken cancellationToken = default);

    Task<bool> CheckNameAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default);

}
