using Volo.Abp.Modularity;

namespace FDSService;

[DependsOn(
    typeof(FDSServiceApplicationModule),
    typeof(FDSServiceDomainTestModule)
    )]
public class FDSServiceApplicationTestModule : AbpModule
{

}
