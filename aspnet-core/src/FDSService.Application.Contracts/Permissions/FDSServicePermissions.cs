namespace FDSService.Permissions;

public static class FDSServicePermissions
{
    public const string GroupName = "FDSService";


    public static class Packages
    {
        public const string Default = GroupName + ".Packages";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }


    public static class Clients
    {
        public const string GroupName = FDSServicePermissions.GroupName + ".Clients";
        public static class Packages
        {
            public const string Default = GroupName + ".Packages";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}
