using FDSService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace FDSService;

[DependsOn(
    typeof(FDSServiceEntityFrameworkCoreTestModule)
    )]
public class FDSServiceDomainTestModule : AbpModule
{

}
