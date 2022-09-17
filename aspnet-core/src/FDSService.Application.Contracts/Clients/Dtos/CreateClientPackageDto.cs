using System;
using System.ComponentModel.DataAnnotations;

namespace FDSService.Clients.Dtos;
public class CreateClientPackageDto
{
    [Required]
    [Display(Name ="Client")]
    public Guid? ClientId { get; set; }
    [Required]
    [Display(Name = "Package")]
    public Guid? PackageId { get; set; }
    [Required]
    [Display(Name = "PackageVersion")]
    public Guid? CurrentVersionId { get; set; }

}
