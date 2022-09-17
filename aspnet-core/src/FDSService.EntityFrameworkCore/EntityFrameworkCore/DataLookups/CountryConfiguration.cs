using FDSService.DataLookups;
using FDSService.EntityFrameworkCore.Modeling;

namespace FDSService.EntityFrameworkCore.DataLookups;
public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable(FDSDbProperties.DbTablePrefix + "Countries", FDSDbProperties.DbSchema);

        builder.Property(t => t.Name)
           .HasMaxLength(CountryConsts.MaxNameLength)
           .IsRequired();

        builder.FDSConfigureByConvention();

    }
}

