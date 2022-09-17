using Volo.Abp;

namespace FDSService.Packages;


public class PackageVersionNameOrNumberAlreadyExistException : BusinessException
{
    public PackageVersionNameOrNumberAlreadyExistException(string name,int VersionNumber) : base(FDSServiceDomainErrorCodes.PackageVersionNameOrVersionNumberAlreadyExists)
    {
        WithData("name", name);
        WithData("version", VersionNumber);
    }

}