using System;
using System.Collections.Generic;
using System.Text;
using FDSService.Localization;
using Volo.Abp.Application.Services;

namespace FDSService;

/* Inherit your application services from this class.
 */
public abstract class FDSServiceAppService : ApplicationService
{
    protected FDSServiceAppService()
    {
        LocalizationResource = typeof(FDSServiceResource);
    }
}
