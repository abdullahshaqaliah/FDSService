using System;
using Volo.Abp.Domain.Entities;

namespace FDSService.EntityFrameworkCore.Modeling;

public static class FDSEntityTypeBuilderExtensions
{
    public static void FDSConfigureByConvention(this EntityTypeBuilder b)
    {
        b.ConfigureByConvention();
        b.TryConfigureGuidId();
    }

    public static void TryConfigureGuidId(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IEntity<Guid>>())
        {
            
            b.Property(nameof(IEntity<Guid>.Id))
                .HasDefaultValueSql("newid()");
        }
    }



}

