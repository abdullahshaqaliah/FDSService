using Volo.Abp;

namespace FDSService.Packages;


public class PackageNameAlreadyExistException : BusinessException
{
    public PackageNameAlreadyExistException(string name) : base(FDSServiceDomainErrorCodes.PackageNameAlreadyExists)
    {
        WithData("name", name);
    }

}