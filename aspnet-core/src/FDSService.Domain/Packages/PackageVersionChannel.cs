using FDSService.DataLookups;
using System;
using Volo.Abp.Domain.Entities;

namespace FDSService.Packages;
public class PackageVersionChannel:Entity<int>
{
    public int ChannelId { get; set; }
    public Channel Channel { get; set; }

    public Guid VersionId { get; set; }

    public PackageVersion Version { get; set; }
}
