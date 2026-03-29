using DomusMind.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.Auth;

public sealed class OperatorInvitationConfiguration : IEntityTypeConfiguration<OperatorInvitation>
{
    public void Configure(EntityTypeBuilder<OperatorInvitation> builder)
    {
        builder.ToTable("operator_invitations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.Token)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Note)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(x => x.ExpiresAtUtc)
            .IsRequired();

        builder.Property(x => x.CreatedByUserId)
            .IsRequired();

        builder.HasIndex(x => x.Token)
            .IsUnique();

        builder.HasIndex(x => x.Email);
    }
}
