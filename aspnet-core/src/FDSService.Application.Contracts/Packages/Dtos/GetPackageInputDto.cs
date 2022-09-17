namespace FDSService.Packages.Dtos;
public class GetPackageInputDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    public bool? IsActive { get; set; }
}
