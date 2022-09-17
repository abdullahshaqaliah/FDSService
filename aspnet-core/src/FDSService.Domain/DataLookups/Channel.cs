using FDSService.Packages;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace FDSService.DataLookups;
public class Channel:Entity<int>
{
    public string Name { get; set; }

    public ICollection<PackageVersionChannel> Versions { get; set; }


}
