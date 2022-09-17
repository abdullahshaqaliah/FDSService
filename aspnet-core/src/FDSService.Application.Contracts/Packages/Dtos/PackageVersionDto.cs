using FDSService.DataFilters;
using System;
using System.Collections.Generic;

namespace FDSService.Packages.Dtos;
public class PackageVersionDto : AuditedEntityDto<Guid>, IIsActive
{
    public string Name { get; set; }
    public Guid PackageId { get; set; }
    public string Package { get; set; }
    public Guid? DependOnVersionId { get; set; }

    public int VersionNumber { get; set; }

    public DateTime AvailableDate { get; set; }
    public bool IsActive { get; set; }
    public PackageVersionType Type { get; set; }
    public string TypeLabel => Type.ToString();

    public string UrlPath { get; set; }

    public Guid? AttachmentId { get; set; }

    public ICollection<int> Countries { get; set; }

    public ICollection<int> Channels { get; set; }

}
