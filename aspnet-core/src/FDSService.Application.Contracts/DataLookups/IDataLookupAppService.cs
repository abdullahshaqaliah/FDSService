using FDSService.DataLookups.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace FDSService.DataLookups;
public interface IDataLookupAppService:IApplicationService
{
    Task<ListResultDto<SelectItemDto>> GetCountiresListAsync();
    Task<ListResultDto<SelectItemDto>> GetChannelsListAsync();

    Task<IEnumerable<SelectItemDto>> GetPackagesListAsync();

    Task<IEnumerable<SelectItemDto>> GetPackageVersionsListAsync(Guid packageId);

}
