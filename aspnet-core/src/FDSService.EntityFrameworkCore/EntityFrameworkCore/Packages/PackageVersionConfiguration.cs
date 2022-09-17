using FDSService.EntityFrameworkCore.Modeling;
using FDSService.Packages;

namespace FDSService.EntityFrameworkCore.Packages;
public class PackageVersionConfiguration : IEntityTypeConfiguration<PackageVersion>
{
    public void Configure(EntityTypeBuilder<PackageVersion> builder)
    {
        builder.ToTable(FDSDbProperties.DbTablePrefix + "PackageVersions", FDSDbProperties.DbSchema);

        builder
            .Property(t => t.Name)
           .HasMaxLength(PackageVersionConsts.MaxNameLength)
           .IsRequired();

        builder
            .HasOne(b => b.Package)
            .WithMany(b => b.Versions)
            .HasForeignKey(b => b.PackageId)
            .IsRequired();


        builder
            .HasOne(b => b.Attachment)
            .WithMany(b => b.Versions)
            .HasForeignKey(b => b.AttachmentId)
            .IsRequired(false);

        builder
            .Property(b => b.AvailableDate)
            .HasColumnType("date");

        builder
            .HasOne(b => b.DependOnVersion)
            .WithMany(b => b.Versions)
            .HasForeignKey(b => b.DependOnVersionId)
            .IsRequired(false);

        builder.FDSConfigureByConvention();

    }
}

