using FDSService.Attachments;
using FDSService.Clients;
using FDSService.DataFilters;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace FDSService.Packages;
public class PackageVersion : FullAuditedEntity<Guid>, IIsActive
{
    
    public string Name { get; set; }
    public Guid PackageId { get; set; }
    public Package Package { get; set; }
    /// <summary>
    /// if null its mean the default version
    /// </summary>
    public Guid? DependOnVersionId { get; set; }
    public PackageVersion DependOnVersion { get; set; }

    public ICollection<PackageVersion> Versions { get; set; }
    public int VersionNumber { get; set; }
    /// <summary>
    /// Available date to  clients is used to check if there are software updates
    /// </summary>
    public DateTime AvailableDate { get; set; }
    public bool IsActive { get; set; }
    public PackageVersionType Type { get; set; }
    public string UrlPath { get; set; }

    public Guid? AttachmentId { get; set; }
    public Attachment Attachment { get; set; }
    public ICollection<PackageVersionCountry> Countries { get; set; }=new List<PackageVersionCountry>();

    public ICollection<PackageVersionChannel> Channels { get; set; } = new List<PackageVersionChannel>();

    public ICollection<ClientPackage> Clients { get; set; }

    public PackageVersion()
    {

    }
    public PackageVersion(Guid id)
    {
        Id = id;
    }

}
