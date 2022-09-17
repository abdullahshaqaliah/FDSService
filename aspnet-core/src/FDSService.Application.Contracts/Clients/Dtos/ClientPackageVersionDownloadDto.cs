using FDSService.Packages;
using System;

namespace FDSService.Clients.Dtos;
public class ClientPackageVersionDownloadDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public int VersionNumber { get; set; }

    public PackageVersionType Type { get; set; }

    public string FileName { get; set; }
}
