using FDSService.Packages;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace FDSService.Clients;
public class ClientPackage : FullAuditedEntity<int>
{
    public Guid ClientId { get; set; }
    public IdentityUser Client { get; set; }

    public Guid PackageId { get; set; }

    public Package Package { get; set; }

    public Guid CurrentVersionId { get; set; }
    public PackageVersion CurrentVersion { get; set; }
    public ClientPackage()
    {

    }
    public ClientPackage(Guid clientId, Guid packageId, Guid currentVersionId)
    {
        ClientId = clientId;
        PackageId = packageId;
        CurrentVersionId = currentVersionId;
    }
}
