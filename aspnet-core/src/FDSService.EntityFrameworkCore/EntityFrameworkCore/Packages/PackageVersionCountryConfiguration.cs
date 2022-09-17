using FDSService.EntityFrameworkCore.Modeling;
using FDSService.Packages;

namespace FDSService.EntityFrameworkCore.Packages;
public class PackageVersionCountryConfiguration : IEntityTypeConfiguration<PackageVersionCountry>
{
    public void Configure(EntityTypeBuilder<PackageVersionCountry> builder)
    {
        builder.ToTable(FDSDbProperties.DbTablePrefix + "PackageVersionCountries", FDSDbProperties.DbSchema);


        builder
            .HasOne(b => b.Country)
            .WithMany(b => b.Versions)
            .HasForeignKey(b => b.CountryId)
            .IsRequired();

        builder
            .HasOne(b => b.Version)
            .WithMany(b => b.Countries)
            .HasForeignKey(b => b.VersionId)
            .IsRequired();

        builder.FDSConfigureByConvention();

    }
}

