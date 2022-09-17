using FDSService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace FDSService.Permissions;

public class FDSServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(FDSServicePermissions.GroupName);
        AddPackagePermissions(myGroup);
        AddClientPackagePermission(myGroup);
    }
    private void AddPackagePermissions(PermissionGroupDefinition group)
    {
        var Permission = group.AddPermission(FDSServicePermissions.Packages.Default, L("Permission:Packages"));
        Permission.AddChild(FDSServicePermissions.Packages.Create, L("Permission:Create"));
        Permission.AddChild(FDSServicePermissions.Packages.Edit, L("Permission:Edit"));
        Permission.AddChild(FDSServicePermissions.Packages.Delete, L("Permission:Delete"));

    }

    private void AddClientPackagePermission(PermissionGroupDefinition group)
    {
        var Permission = group.AddPermission(FDSServicePermissions.Clients.Packages.Default, L("Permission:ClientPackages"));
        Permission.AddChild(FDSServicePermissions.Clients.Packages.Create, L("Permission:Create"));
        Permission.AddChild(FDSServicePermissions.Clients.Packages.Delete, L("Permission:Delete"));
    }
    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FDSServiceResource>(name);
    }
}
