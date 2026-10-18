using DomusMind.Domain.Calendar;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.Calendar;

public sealed class CalendarSubscriptionConfiguration : IEntityTypeConfiguration<CalendarSubscription>
{
    public void Configure(EntityTypeBuilder<CalendarSubscription> builder)
    {
        builder.ToTable("calendar_subscriptions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => CalendarSubscriptionId.From(x))
            .HasColumnName("id");
        builder.Property(x => x.FamilyId).HasColumnName("family_id").IsRequired();
        builder.Property(x => x.OwnerUserId).HasColumnName("owner_user_id").IsRequired();
        builder.Property(x => x.Label).HasColumnName("label").HasMaxLength(100).IsRequired();
        builder.Property(x => x.MemberId).HasColumnName("member_id");
        builder.Property(x => x.TokenHash).HasColumnName("token_hash").HasMaxLength(64).IsRequired();
        builder.HasIndex(x => x.TokenHash).IsUnique();
        builder.HasIndex(x => new { x.FamilyId, x.OwnerUserId });
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.ExpiresAtUtc).HasColumnName("expires_at_utc").IsRequired();
        builder.Property(x => x.RevokedAtUtc).HasColumnName("revoked_at_utc");
        builder.Property(x => x.LastAccessedAtUtc).HasColumnName("last_accessed_at_utc");
        builder.Property(x => x.AccessCount).HasColumnName("access_count").IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}
