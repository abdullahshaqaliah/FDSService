using FDSService.DataLookups;
using System;
using Volo.Abp.Domain.Entities;

namespace FDSService.Packages;
public class PackageVersionCountry : Entity<int>
{
    public int CountryId { get; set; }
    public Country Country { get; set; }

    public Guid VersionId { get; set; }

    public PackageVersion Version { get; set; }

}
