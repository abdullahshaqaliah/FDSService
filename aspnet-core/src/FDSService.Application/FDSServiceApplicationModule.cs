using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace FDSService;

[DependsOn(
    typeof(FDSServiceDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(FDSServiceApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpBlobStoringFileSystemModule)

    )]
public class FDSServiceApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        ConfigureStorageService(configuration);

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<FDSServiceApplicationModule>();
        });
    }

    private void ConfigureStorageService(IConfiguration configuration)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), configuration["StorageService:path"]);
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.UseFileSystem(fileSystem =>
                {
                    fileSystem.BasePath = path;
                });
            });
        });
    }
}
