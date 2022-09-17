using FDSService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace FDSService.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class FDSServiceController : AbpControllerBase
{
    protected FDSServiceController()
    {
        LocalizationResource = typeof(FDSServiceResource);
    }
}
