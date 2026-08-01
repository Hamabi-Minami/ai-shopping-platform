using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShippingPlatform.Domain.Entities;

namespace MyShippingPlatform.Infrastructure.Persistence.Configurations;

public class AIAuditRecordConfiguration : IEntityTypeConfiguration<AIAuditRecord>
{
    public void Configure(EntityTypeBuilder<AIAuditRecord> builder)
    {
        // 1. Primary key configuration
        builder.HasKey(a => a.Id);

        // 2. MySQL specific configuration: Store raw AI output as native JSON type
        builder.Property(a => a.RawAgentOutputJson)
               .HasColumnType("json")
               .IsRequired(false); // Set to false if it can be null upon initial creation

        // 3. Financial data precision control (decimal(18,2) is compatible with MySQL)
        builder.Property(a => a.DeclaredValue)
               .HasColumnType("decimal(18,2)");

        // 4. String length limits to prevent default varchar(longtext) mapping
        builder.Property(a => a.ExtractedHsCode)
               .HasMaxLength(32);

        builder.Property(a => a.RiskReasoning)
               .HasMaxLength(1000);

        builder.Property(a => a.ReviewedByUserId)
               .HasMaxLength(128);
    }
}
