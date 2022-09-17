using FDSService.DataFilters;
using FDSService.DataLookups.Dtos;
using FDSService.Packages;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;

namespace FDSService.DataLookups;
[Authorize]
public class DataLookupAppService : FDSServiceAppService, IDataLookupAppService
{
    private readonly IRepository<Country, int> _countryRepository;
    private readonly IRepository<Channel, int> _channelRepository;
    private readonly IRepository<Package, Guid> _packageRepository;
    private readonly IRepository<PackageVersion, Guid> _versionRepository;

    private readonly IDataFilter<IIsActive> _isActiveFilter;


    public DataLookupAppService(IRepository<Country, int> countryRepository, IRepository<Channel, int> channelRepository, IRepository<Package, Guid> packageRepository, IRepository<PackageVersion, Guid> versionRepository, IDataFilter<IIsActive> isActiveFilter)
    {
        _countryRepository = countryRepository;
        _channelRepository = channelRepository;
        _packageRepository = packageRepository;
        _versionRepository = versionRepository;
        _isActiveFilter = isActiveFilter;
    }

    public virtual async Task<ListResultDto<SelectItemDto>> GetCountiresListAsync()
    {
        return new ListResultDto<SelectItemDto>((await _countryRepository.GetListAsync().ConfigureAwait(false)).Select(c => new SelectItemDto(c.Id.ToString(), c.Name)).ToList());
    }

    public virtual async Task<ListResultDto<SelectItemDto>> GetChannelsListAsync()
    {
        return new ListResultDto<SelectItemDto>((await _channelRepository.GetListAsync().ConfigureAwait(false)).Select(c => new SelectItemDto(c.Id.ToString(), c.Name)).ToList());
    }

    public virtual async Task<IEnumerable<SelectItemDto>> GetPackagesListAsync()
    {
        return (await _packageRepository.GetListAsync().ConfigureAwait(false)).Select(c => new SelectItemDto(c.Id.ToString(), c.Name));
    }

    public virtual async Task<IEnumerable<SelectItemDto>> GetPackageVersionsListAsync(Guid packageId)
    {
        var Queryable = await _versionRepository.GetQueryableAsync().ConfigureAwait(false);
        return Queryable.Where(v => v.PackageId == packageId).Select(v => new SelectItemDto(v.Id.ToString(), v.Name)).ToList();

    }


}