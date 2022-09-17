using System;

namespace FDSService.Clients.Dtos;
public class ClientPackageDto : AuditedEntityDto<int>
{
    public Guid ClientId { get; set; }
    public Guid PackageId { get; set; }
    public string Package { get; set; }
    public string CurrentVersion { get; set; }
}
