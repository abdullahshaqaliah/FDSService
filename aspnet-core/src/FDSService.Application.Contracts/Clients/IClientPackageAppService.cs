using FDSService.Clients.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace FDSService.Clients;
public interface IClientPackageAppService : IApplicationService
{
    Task<PagedResultDto<ClientPackageDto>> GetListAsync(GetClientPackageInputDto input);
    Task<PagedResultDto<ClientPackageDto>> GetMyPackageListAsync(PagedAndSortedResultRequestDto input);
    Task<ListResultDto<ClientPackageVersionDownloadDto>> GetPackageVersionCanUpdateListAsync(Guid packageId);
    Task<IRemoteStreamContent> DownloadFileAsync(Guid id);
    Task<string> DownloadFromUrlAsync(Guid id);
    Task<ClientPackageDto> CreateAsync(CreateClientPackageDto input);
    Task DeleteAsync(int id);

}