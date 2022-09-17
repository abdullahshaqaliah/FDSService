using FDSService.Attachments;
using FDSService.DataFilters;
using FDSService.Packages;
using FDSService.Packages.Dtos;
using FDSService.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;

namespace FDSService.PackageVersions;
public class PackageVersionAppService : CrudAppService<
        PackageVersion,
        PackageVersionDto,
        Guid,
        GetPackageVersionInputDto,
        CreateOrUpdatePackageVersionDto>, IPackageVersionAppService
{
    private readonly IDataFilter<IIsActive> _isActiveFilter;

    private readonly IPackageVersionRepository _packageVersionRepository;
    private readonly IBlobContainer<PackageVersionContainer> _blobContainer;
    private readonly IRepository<Attachment, Guid> _attachmentRepository;

    public PackageVersionAppService(IRepository<PackageVersion, Guid> repository, IDataFilter<IIsActive> isActiveFilter, IPackageVersionRepository PackageVersionRepository, IBlobContainer<PackageVersionContainer> blobContainer, IRepository<Attachment, Guid> attachmentRepository) : base(repository)
    {

        _isActiveFilter = isActiveFilter;
        _isActiveFilter.Disable();
        _packageVersionRepository = PackageVersionRepository;


        GetPolicyName = FDSServicePermissions.Packages.Default;
        GetListPolicyName = FDSServicePermissions.Packages.Default;
        CreatePolicyName = FDSServicePermissions.Packages.Create;
        UpdatePolicyName = FDSServicePermissions.Packages.Edit;
        DeletePolicyName = FDSServicePermissions.Packages.Delete;
        _blobContainer = blobContainer;
        _attachmentRepository = attachmentRepository;
    }
    public override async Task<PagedResultDto<PackageVersionDto>> GetListAsync(GetPackageVersionInputDto input)
    {
        var (result, totalCount) = await _packageVersionRepository.GetListAsync(input.Sorting, input.SkipCount, input.MaxResultCount, input.Filter, input.PackageId, input.CountryId, input.ChannelId, input.Type);
        return new PagedResultDto<PackageVersionDto>(totalCount, ObjectMapper.Map<List<PackageVersion>, List<PackageVersionDto>>(result));
    }


    public override async Task<PackageVersionDto> CreateAsync(CreateOrUpdatePackageVersionDto input)
    {
        await CheckIsExistsAsync(input).ConfigureAwait(false);
       if (input.Type== PackageVersionType.File)
        {
            await SaveVersionFileAsync(input).ConfigureAwait(false);
        }
        return await base.CreateAsync(input).ConfigureAwait(false);
    }
    [Authorize(FDSServicePermissions.Packages.Edit)]
    public override async Task<PackageVersionDto> UpdateAsync(Guid id, CreateOrUpdatePackageVersionDto input)
    {
        if (input.DependOnVersionId.HasValue && input.DependOnVersionId.Value== id)
        {
            throw new BusinessException(FDSServiceDomainErrorCodes.VersionDependencyIsRelatedWithAnotherVersion);

        }
        var entity = await GetEntityByIdAsync(id).ConfigureAwait(false);
        if (entity == null) return default;

        await CheckIsExistsAsync(input, id).ConfigureAwait(false);
        if (input.Type == PackageVersionType.File)
        {
            await SaveVersionFileAsync(input).ConfigureAwait(false);

        }
        else if (input.AttachmentId.HasValue)
        {
            await DeleteFileAsync(input).ConfigureAwait(false);
        }

        entity.Countries.Clear();
        entity.Channels.Clear();
        await MapToEntityAsync(input, entity).ConfigureAwait(false);

        await Repository.UpdateAsync(entity).ConfigureAwait(false);

        return await MapToGetOutputDtoAsync(entity);

    }
    private async Task CheckIsExistsAsync(CreateOrUpdatePackageVersionDto input, Guid? expectedId = null)
    {
        if (await _packageVersionRepository.CheckNameOrNumberAsync(input.Name, input.VersionNumber, input.PackageId, expectedId).ConfigureAwait(false))
        {
            throw new PackageVersionNameOrNumberAlreadyExistException(input.Name, input.VersionNumber);

        }
        else if ( await CheckHasDefaultVersionAsync(input, expectedId).ConfigureAwait(false))
        {
            throw new BusinessException(FDSServiceDomainErrorCodes.PackageHasDefaultVersion);

        }
        else if (await CheckIfVersionDependencyIsRelatedWithAnotherVersionAsync(input.DependOnVersionId, expectedId).ConfigureAwait(false))
        {
            throw new BusinessException(FDSServiceDomainErrorCodes.VersionDependencyIsRelatedWithAnotherVersion);

        }
    }

    private async Task<bool> CheckHasDefaultVersionAsync(CreateOrUpdatePackageVersionDto input, Guid? expectedId = null)
    {
        if (!input.DependOnVersionId.HasValue)
        {
           return await _packageVersionRepository.CheckHasDefaultVersionAsync(input.PackageId, expectedId).ConfigureAwait(false);         
        }
        return false;
    }
    private async Task<bool> CheckIfVersionDependencyIsRelatedWithAnotherVersionAsync(Guid? dependOnVersionId, Guid? expectedId = null)
    {
        if (dependOnVersionId.HasValue)
        {
            return await _packageVersionRepository.CheckIfVersionDependencyIsRelatedWithAnotherVersionAsync(dependOnVersionId.Value, expectedId).ConfigureAwait(false);
        }
        return false;
    } 

    private async Task SaveVersionFileAsync(CreateOrUpdatePackageVersionDto input)
    {
        
        if (input.Content !=null && input.Content.ContentLength>0)
        {
            Attachment attachment = new Attachment
             (
                Path.GetFileName(input.Content.FileName),
                input.Content.ContentLength.Value,
                input.Content.ContentType,
                Path.GetExtension(input.Content.FileName)
             );
            await _attachmentRepository.InsertAsync(attachment, true);
            if (input.AttachmentId.HasValue)
            {
                await _attachmentRepository.DeleteAsync(input.AttachmentId.Value).ConfigureAwait(false);
                await Task.WhenAll
                    (
                        _blobContainer.SaveAsync(attachment.Id.ToString(), input.Content.GetStream(), true),
                        _blobContainer.DeleteAsync(input.AttachmentId.Value.ToString())
                    ).ConfigureAwait(false);
            }
            else
            {
                await _blobContainer.SaveAsync(attachment.Id.ToString(), input.Content.GetStream(), true).ConfigureAwait(false);

            }
            input.AttachmentId = attachment.Id;
        }
    }
    private async Task DeleteFileAsync(CreateOrUpdatePackageVersionDto input)
    {
        await _attachmentRepository.DeleteAsync(input.AttachmentId.Value);
        await _blobContainer.DeleteAsync(input.AttachmentId.Value.ToString()).ConfigureAwait(false);
        input.AttachmentId = default;

    }
}
