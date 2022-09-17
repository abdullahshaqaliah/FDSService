using FDSService.EntityFrameworkCore.Modeling;
using FDSService.Packages;

namespace FDSService.EntityFrameworkCore.Packages;
public class PackageConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder.ToTable(FDSDbProperties.DbTablePrefix + "Packages", FDSDbProperties.DbSchema);

        builder.Property(t => t.Name)
           .HasMaxLength(PackageConsts.MaxNameLength)
           .IsRequired();

        builder.FDSConfigureByConvention();

    }
}

