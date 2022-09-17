using FDSService.Packages;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace FDSService.Attachments;
public class Attachment : CreationAuditedEntity<Guid>
{
    public  string Name { get; set; }
    public long Size { get; set; }
    public  string ContentType { get; set; }
    public  string Extension { get; set; }

    public ICollection<PackageVersion> Versions { get; set; }

    public Attachment()
    {

    }
    public Attachment(string name, long size, string contentType, string extension)
    {
        Name = name;
        Size = size;
        ContentType = contentType;
        Extension = extension;
    }
}
