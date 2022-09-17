using FDSService.DataFilters;
using FDSService.Packages.Dtos;
using FDSService.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;

namespace FDSService.Packages;
public class PackageAppService : CrudAppService<
        Package,
        PackageDto,
        Guid,
        GetPackageInputDto,
        CreateOrUpdatePackageDto>, IPackageAppService
{
    private readonly IDataFilter<IIsActive> _isActiveFilter;

    private readonly IPackageRepository _packageRepository;
    public PackageAppService(IRepository<Package, Guid> repository, IDataFilter<IIsActive> isActiveFilter, IPackageRepository packageRepository) : base(repository)
    {

        _isActiveFilter = isActiveFilter;
        _isActiveFilter.Disable();
        _packageRepository = packageRepository;


        GetPolicyName =     FDSServicePermissions.Packages.Default;
        GetListPolicyName = FDSServicePermissions.Packages.Default;
        CreatePolicyName =  FDSServicePermissions.Packages.Create;
        UpdatePolicyName =  FDSServicePermissions.Packages.Edit;
        DeletePolicyName =  FDSServicePermissions.Packages.Delete;
    }
    public override async Task<PagedResultDto<PackageDto>> GetListAsync(GetPackageInputDto input)
    {


        var (result, totalCount) = await _packageRepository.GetListAsync(input.Sorting, input.SkipCount, input.MaxResultCount, input.Filter, input.IsActive);
        return new PagedResultDto<PackageDto>(totalCount, ObjectMapper.Map<List<Package>, List<PackageDto>>(result));
    }


    public override async Task<PackageDto> CreateAsync(CreateOrUpdatePackageDto input)
    {
        await CheckIsExistsAsync(input);
        return await base.CreateAsync(input);
    }
    public override async Task<PackageDto> UpdateAsync(Guid id, CreateOrUpdatePackageDto input)
    {
        await CheckIsExistsAsync(input, id);
        return await base.UpdateAsync(id, input);
    }
    private async Task CheckIsExistsAsync(CreateOrUpdatePackageDto input, Guid? expectedId = null)
    {
        if (await _packageRepository.CheckNameAsync(input.Name, expectedId))
        {
            throw new PackageNameAlreadyExistException(input.Name);

        }
    }
}
