using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace FDSService;

[Dependency(ReplaceServices = true)]
public class FDSServiceBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "FDSService";
}
