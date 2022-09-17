using FDSService.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;

namespace FDSService.Packages.Dtos;
public class CreateOrUpdatePackageVersionDto:IValidatableObject
{
    [Required]
    [StringLength(PackageVersionConsts.MaxNameLength)]
    public string Name { get; set; }
    public Guid PackageId { get; set; }
    public Guid? DependOnVersionId { get; set; }
    public int VersionNumber { get; set; }

    [Required]
    public DateTime? AvailableDate { get; set; }
    public bool IsActive { get; set; }
    public PackageVersionType Type { get; set; }

    [Url]
    [StringLength(PackageVersionConsts.MaxUrlPathLength)]
    [Display(Name = "UrlPath")]
    public string UrlPath { get; set; }
    public Guid? AttachmentId { get; set; }
    public IRemoteStreamContent Content { get; set; }
    public List<int> Channels { get; set; }
    public List<int> Countries { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var l = validationContext.GetRequiredService<IStringLocalizer<FDSServiceResource>>();

        if (Countries.Count ==0)
        {

                yield return new ValidationResult
                (
                     string.Format(l[FDSServiceDomainErrorCodes.IsRequired].Value, l["Country"].Value),
                        new[] { nameof(Countries), }
                );


        }

        if (Channels.Count == 0)
        {
            yield return new ValidationResult
            (
                 string.Format(l[FDSServiceDomainErrorCodes.IsRequired].Value, l["Channel"].Value),
                    new[] { nameof(Channels), }
            );
        }

      
        
        switch (Type)
        {
            case PackageVersionType.Url:
                if (UrlPath.IsNullOrWhiteSpace())
                {
                    yield return new ValidationResult
                    (
                         string.Format(l[FDSServiceDomainErrorCodes.IsRequired].Value, l["UrlPath"].Value),
                            new[] { nameof(UrlPath), }
                    );
                }
                break;
            case PackageVersionType.File:
                if (!AttachmentId.HasValue && Content== null)
                {
                    yield return new ValidationResult
                    (
                         string.Format(l[FDSServiceDomainErrorCodes.IsRequired].Value, l["Attachment"].Value),
                            new[] { nameof(AttachmentId), }
                    );
                }
                break;
        }

    }
}
