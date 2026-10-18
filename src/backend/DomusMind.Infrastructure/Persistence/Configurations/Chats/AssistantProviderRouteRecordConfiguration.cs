using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.Chats;

public sealed class AssistantProviderRouteRecordConfiguration
    : IEntityTypeConfiguration<AssistantProviderRouteRecord>
{
    public void Configure(EntityTypeBuilder<AssistantProviderRouteRecord> builder)
    {
        builder.ToTable("assistant_provider_routes");
        builder.HasKey(x => x.FamilyId);
        builder.Property(x => x.FamilyId).HasColumnName("family_id");
        builder.Property(x => x.TargetsJson).HasColumnName("targets_json").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");
    }
}
