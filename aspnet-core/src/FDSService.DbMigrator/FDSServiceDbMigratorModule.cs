using FDSService.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace FDSService.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(FDSServiceEntityFrameworkCoreModule),
    typeof(FDSServiceApplicationContractsModule)
    )]
public class FDSServiceDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
