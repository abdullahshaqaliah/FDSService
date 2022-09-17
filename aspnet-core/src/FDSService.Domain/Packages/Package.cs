using FDSService.Clients;
using FDSService.DataFilters;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace FDSService.Packages;
public class Package : FullAuditedEntity<Guid>, IIsActive
{
    public Package() { }
    public Package( Guid id, string name, bool isActive)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
    }

    public string Name { get; set; }
    public bool IsActive { get; set; }
    public ICollection<PackageVersion> Versions { get; set; }

    public ICollection<ClientPackage> Clients { get; set; }

}
