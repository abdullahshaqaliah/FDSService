using FDSService.DataFilters;
using System.ComponentModel.DataAnnotations;

namespace FDSService.Packages.Dtos;
public class CreateOrUpdatePackageDto : IIsActive
{
    [Required]
    [StringLength(PackageConsts.MaxNameLength)]
    public string Name { get; set; }    
    public bool IsActive { get; set; }

}
