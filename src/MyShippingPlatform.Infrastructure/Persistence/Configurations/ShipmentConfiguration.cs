using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShippingPlatform.Domain.Entities;

namespace MyShippingPlatform.Infrastructure.Persistence.Configurations;

public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.TrackingNumber).HasMaxLength(64).IsRequired();
        builder.Property(s => s.ShipperName).HasMaxLength(128);

        // 1:N Relationship Configuration
        builder.HasMany(s => s.AuditRecords)
               .WithOne(a => a.Shipment)
               .HasForeignKey(a => a.ShipmentId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
