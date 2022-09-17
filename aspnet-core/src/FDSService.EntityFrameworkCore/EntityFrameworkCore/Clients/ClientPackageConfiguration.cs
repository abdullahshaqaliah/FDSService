using FDSService.Clients;
using FDSService.EntityFrameworkCore.Modeling;

namespace FDSService.EntityFrameworkCore.Clients;
public class ClientPackageConfiguration : IEntityTypeConfiguration<ClientPackage>
{
    public void Configure(EntityTypeBuilder<ClientPackage> builder)
    {
        builder.ToTable(FDSDbProperties.DbTablePrefix + "ClientPackages", FDSDbProperties.DbSchema);


        builder
            .HasOne(b => b.Package)
            .WithMany(b => b.Clients)
            .HasForeignKey(b => b.PackageId)
            .IsRequired();

        builder
            .HasOne(b => b.CurrentVersion)
            .WithMany(b => b.Clients)
            .HasForeignKey(b => b.CurrentVersionId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder
            .HasOne(b => b.Client)
            .WithMany()
            .HasForeignKey(b => b.ClientId)
            .IsRequired();

       

        builder.FDSConfigureByConvention();

    }
}

