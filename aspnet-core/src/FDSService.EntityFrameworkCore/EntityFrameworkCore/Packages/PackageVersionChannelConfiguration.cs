using FDSService.EntityFrameworkCore.Modeling;
using FDSService.Packages;

namespace FDSService.EntityFrameworkCore.Packages;
public class PackageVersionChannelConfiguration : IEntityTypeConfiguration<PackageVersionChannel>
{
    public void Configure(EntityTypeBuilder<PackageVersionChannel> builder)
    {
        builder.ToTable(FDSDbProperties.DbTablePrefix + "PackageVersionChannels", FDSDbProperties.DbSchema);



        builder
            .HasOne(b => b.Channel)
            .WithMany(b => b.Versions)
            .HasForeignKey(b => b.ChannelId)
            .IsRequired();

        builder
            .HasOne(b => b.Version)
            .WithMany(b => b.Channels)
            .HasForeignKey(b => b.VersionId)
            .IsRequired();

        builder.FDSConfigureByConvention();

    }
}

