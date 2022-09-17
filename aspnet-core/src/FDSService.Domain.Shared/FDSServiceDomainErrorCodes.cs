namespace FDSService;

public static class FDSServiceDomainErrorCodes
{
    public const string PackageNameAlreadyExists = "FDSService:010001";
    public const string PackageVersionNameOrVersionNumberAlreadyExists = "FDSService:010002";
    public const string PackageHasDefaultVersion = "FDSService:010003";
    public const string VersionDependencyIsRelatedWithAnotherVersion = "FDSService:010004";
    public const string CanNotSelectSameVersionOnDependencyFiled = "FDSService:010005";
    public const string ThePackageAlreadyAddToClient = "FDSService:010006";
    public const string NoPermissionToAccessPackageVersion = "FDSService:010007";

    public const string IsRequired = "The {0} field is required.";



}
