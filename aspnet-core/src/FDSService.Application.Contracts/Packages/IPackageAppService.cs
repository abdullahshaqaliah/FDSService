using FDSService.Packages.Dtos;
using System;
using Volo.Abp.Application.Services;

namespace FDSService.Packages;
public interface IPackageAppService: ICrudAppService<
            PackageDto,
            Guid,
            GetPackageInputDto,
            CreateOrUpdatePackageDto>
{
}
