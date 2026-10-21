using DomusMind.Infrastructure.Persistence.Mcp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations;

public sealed class McpWriteApprovalRecordConfiguration : IEntityTypeConfiguration<McpWriteApprovalRecord>
{
    public void Configure(EntityTypeBuilder<McpWriteApprovalRecord> builder)
    {
        builder.ToTable("mcp_write_approvals");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.OperationNamespace).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
        builder.Property(x => x.IdempotencyKey).HasMaxLength(128).IsRequired();
        builder.Property(x => x.PayloadHash).HasMaxLength(64).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Quantity).HasMaxLength(100);
        builder.Property(x => x.Note).HasMaxLength(500);
        builder.Property(x => x.Status).HasMaxLength(30).IsRequired();
        builder.HasIndex(x => new
        {
            x.OperationNamespace,
            x.UserId,
            x.ClientId,
            x.FamilyId,
            x.IdempotencyKey
        }).IsUnique();
    }
}

public sealed class McpIdempotencyRecordConfiguration : IEntityTypeConfiguration<McpIdempotencyRecord>
{
    public void Configure(EntityTypeBuilder<McpIdempotencyRecord> builder)
    {
        builder.ToTable("mcp_write_idempotency");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.OperationNamespace).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
        builder.Property(x => x.IdempotencyKey).HasMaxLength(128).IsRequired();
        builder.Property(x => x.PayloadHash).HasMaxLength(64).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(30).IsRequired();
        builder.Property(x => x.ResultJson).HasColumnType("jsonb");
        builder.HasIndex(x => new
        {
            x.OperationNamespace,
            x.UserId,
            x.ClientId,
            x.FamilyId,
            x.IdempotencyKey
        }).IsUnique();
    }
}
