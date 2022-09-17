using FDSService.Packages;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace FDSService.DataLookups;
public class Country : Entity<int>
{
    public string Name { get; set; }

    public ICollection<PackageVersionCountry> Versions { get; set; }

}
