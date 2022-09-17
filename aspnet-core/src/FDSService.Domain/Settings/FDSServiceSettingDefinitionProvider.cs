using Volo.Abp.Settings;

namespace FDSService.Settings;

public class FDSServiceSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(FDSServiceSettings.MySetting1));
    }
}
