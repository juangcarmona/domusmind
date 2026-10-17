using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.Calendar;

public sealed class ExternalCalendarOAuthTransactionConfiguration
    : IEntityTypeConfiguration<ExternalCalendarOAuthTransaction>
{
    public void Configure(EntityTypeBuilder<ExternalCalendarOAuthTransaction> builder)
    {
        builder.ToTable("external_calendar_oauth_transactions");

        builder.HasKey(transaction => transaction.StateHash);

        builder.Property(transaction => transaction.StateHash)
            .HasColumnName("state_hash")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(transaction => transaction.RequestedByUserId)
            .HasColumnName("requested_by_user_id")
            .IsRequired();

        builder.Property(transaction => transaction.FamilyId)
            .HasColumnName("family_id")
            .IsRequired();

        builder.Property(transaction => transaction.MemberId)
            .HasColumnName("member_id")
            .IsRequired();

        builder.Property(transaction => transaction.RedirectUri)
            .HasColumnName("redirect_uri")
            .HasMaxLength(2048)
            .IsRequired();

        builder.Property(transaction => transaction.ProtectedPkceVerifier)
            .HasColumnName("protected_pkce_verifier")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(transaction => transaction.ExpiresAtUtc)
            .HasColumnName("expires_at_utc")
            .IsRequired();

        builder.HasIndex(transaction => transaction.ExpiresAtUtc)
            .HasDatabaseName("ix_external_calendar_oauth_transactions_expires_at_utc");
    }
}
