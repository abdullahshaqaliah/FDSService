using FDSService.DataFilters;
using System;
using System.Collections.Generic;
using System.Text;

namespace FDSService.Packages.Dtos;
public class PackageDto : AuditedEntityDto<Guid>, IIsActive
{
    public string Name { get; set; }

    public bool IsActive { get; set; }
}
