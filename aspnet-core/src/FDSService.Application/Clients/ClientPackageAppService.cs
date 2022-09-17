using FDSService.Clients.Dtos;
using FDSService.Packages;
using FDSService.Packages.Dtos;
using FDSService.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace FDSService.Clients;
[Authorize()]
public class ClientPackageAppService : FDSServiceAppService, IClientPackageAppService
{
    private readonly IClientPackageRepository _repository;
    private readonly IBlobContainer<PackageVersionContainer> _blobContainer;
    public ClientPackageAppService(IClientPackageRepository repository, IBlobContainer<PackageVersionContainer> blobContainer)
    {
        _repository = repository;
        _blobContainer = blobContainer;
    }
    [Authorize(FDSServicePermissions.Clients.Packages.Default)]
    public virtual async Task<PagedResultDto<ClientPackageDto>> GetListAsync(GetClientPackageInputDto input)
    {
        var (result, totalCount) = await _repository.GetListAsync(input.Sorting, input.SkipCount, input.MaxResultCount, input.ClientId);
        return new PagedResultDto<ClientPackageDto>(totalCount, ObjectMapper.Map<List<ClientPackage>, List<ClientPackageDto>>(result));
    }

    public virtual async Task<PagedResultDto<ClientPackageDto>> GetMyPackageListAsync(PagedAndSortedResultRequestDto input)
    {
        var (result, totalCount) = await _repository.GetListAsync(input.Sorting, input.SkipCount, input.MaxResultCount, CurrentUser.GetId());
        return new PagedResultDto<ClientPackageDto>(totalCount, ObjectMapper.Map<List<ClientPackage>, List<ClientPackageDto>>(result));
    }
    public virtual async Task<ListResultDto<ClientPackageVersionDownloadDto>> GetPackageVersionCanUpdateListAsync(Guid packageId)
    {
        var result = await _repository.GetPackageVersionsUpdatedListAsync(packageId).ConfigureAwait(false);
        return new ListResultDto<ClientPackageVersionDownloadDto>(ObjectMapper.Map<List<PackageVersion>, List<ClientPackageVersionDownloadDto>>(result));
    }

    [Authorize(FDSServicePermissions.Clients.Packages.Create)]
    public virtual async Task<ClientPackageDto> CreateAsync(CreateClientPackageDto input)
    {
        if (await _repository.CheckPackageIsExists(input.ClientId.Value, input.PackageId.Value).ConfigureAwait(false))
        {
            throw new BusinessException(FDSServiceDomainErrorCodes.ThePackageAlreadyAddToClient);

        }
        var clientPackage = ObjectMapper.Map<CreateClientPackageDto, ClientPackage>(input);
        await _repository.InsertAsync(clientPackage).ConfigureAwait(false);

        return ObjectMapper.Map<ClientPackage, ClientPackageDto>(clientPackage);

    }
    [Authorize(FDSServicePermissions.Clients.Packages.Delete)]
    public virtual async Task DeleteAsync(int id)
    {
        var clientPackage = await _repository.FindAsync(id);
        if (clientPackage == null)
        {
            return;
        }
        await _repository.DeleteAsync(clientPackage).ConfigureAwait(false);
    }



    public virtual async Task<IRemoteStreamContent> DownloadFileAsync(Guid id)
    {
        var version = await GetVetsionCanDownloadAsync(id).ConfigureAwait(false);

        await UpdateVetsionAsync(version).ConfigureAwait(false);

        var fs = await _blobContainer.GetAsync(version.AttachmentId.Value.ToString()).ConfigureAwait(false);
        return await Task.FromResult(
               (IRemoteStreamContent)new RemoteStreamContent(fs)
               {

               }
           );
    }

    public virtual async Task<string> DownloadFromUrlAsync(Guid id)
    {
        var version = await GetVetsionCanDownloadAsync(id).ConfigureAwait(false);

        await UpdateVetsionAsync(version).ConfigureAwait(false);

        return version.UrlPath;
    }

    private async Task<PackageVersion> GetVetsionCanDownloadAsync(Guid id)
    {
        var version = await _repository.GetPackageVersionUpdatedAsync(id).ConfigureAwait(false);
        if (version == null)
        {
            throw new BusinessException(FDSServiceDomainErrorCodes.NoPermissionToAccessPackageVersion);
        }
        return version;
    }
    private async Task UpdateVetsionAsync(PackageVersion version)
    {
        var currentVersion = await _repository.FirstOrDefaultAsync(x => x.PackageId == version.PackageId && x.ClientId == CurrentUser.GetId());
        currentVersion.CurrentVersionId = version.Id;
    }

}