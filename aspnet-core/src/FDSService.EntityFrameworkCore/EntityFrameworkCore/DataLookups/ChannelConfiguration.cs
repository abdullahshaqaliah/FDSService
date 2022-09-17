using FDSService.DataLookups;
using FDSService.EntityFrameworkCore.Modeling;

namespace FDSService.EntityFrameworkCore.DataLookups;
public class ChannelConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.ToTable(FDSDbProperties.DbTablePrefix + "Channels", FDSDbProperties.DbSchema);

        builder.Property(t => t.Name)
           .HasMaxLength(ChannelConsts.MaxNameLength)
           .IsRequired();

        builder.FDSConfigureByConvention();

    }
}

