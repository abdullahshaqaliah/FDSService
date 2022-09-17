using FDSService.Packages.Dtos;
using System;
using Volo.Abp.Application.Services;

namespace FDSService.PackageVersions;
public interface IPackageVersionAppService : ICrudAppService<
            PackageVersionDto,
            Guid,
            GetPackageVersionInputDto,
            CreateOrUpdatePackageVersionDto>
    {

    }
