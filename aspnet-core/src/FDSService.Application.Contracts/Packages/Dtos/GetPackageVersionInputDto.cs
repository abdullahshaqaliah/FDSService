using System;

namespace FDSService.Packages.Dtos;
public class GetPackageVersionInputDto: PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
    public Guid? PackageId { get; set; }
    public int? CountryId { get; set; }

    public int? ChannelId { get; set; }
    public PackageVersionType? Type { get; set; }
}
