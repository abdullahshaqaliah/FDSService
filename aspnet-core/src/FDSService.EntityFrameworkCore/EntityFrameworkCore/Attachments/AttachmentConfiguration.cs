using FDSService.EntityFrameworkCore.Modeling;
using FDSService.Attachments;

namespace FDSService.EntityFrameworkCore.Attachments;
public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.ToTable(FDSDbProperties.DbTablePrefix + "Attachments", FDSDbProperties.DbSchema);

        builder.Property(t => t.Name)
           .HasMaxLength(AttachmentConsts.MaxNameLength)
           .IsRequired();

        builder.Property(t => t.ContentType)
           .HasMaxLength(AttachmentConsts.MaxContentTypeLength)
           .IsRequired();

        builder.Property(t => t.Extension)
           .HasMaxLength(AttachmentConsts.MaxExtensionLength)
           .IsRequired();
        builder.FDSConfigureByConvention();

    }
}

