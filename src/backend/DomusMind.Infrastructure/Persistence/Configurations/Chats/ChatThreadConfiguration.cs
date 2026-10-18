using DomusMind.Domain.Chats;
using DomusMind.Domain.Chats.ValueObjects;
using DomusMind.Domain.Family;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.Chats;

public sealed class ChatThreadConfiguration : IEntityTypeConfiguration<ChatThread>
{
    public void Configure(EntityTypeBuilder<ChatThread> builder)
    {
        builder.ToTable("chat_threads");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => ChatThreadId.From(x)).HasColumnName("id");
        builder.Property(x => x.FamilyId).HasConversion(x => x.Value, x => FamilyId.From(x)).HasColumnName("family_id");
        builder.Property(x => x.Title).HasColumnName("title").HasMaxLength(120).IsRequired();
        builder.Property(x => x.Kind).HasColumnName("kind").HasMaxLength(20).IsRequired();
        builder.Property(x => x.CreatedByMemberId).HasColumnName("created_by_member_id");
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc");
        builder.HasIndex(x => new { x.FamilyId, x.CreatedAtUtc });
        builder.OwnsMany(x => x.Participants, participant =>
        {
            participant.ToTable("chat_thread_participants");
            participant.WithOwner().HasForeignKey("ChatThreadId");
            participant.Property<ChatThreadId>("ChatThreadId")
                .HasConversion(x => x.Value, x => ChatThreadId.From(x))
                .HasColumnName("chat_thread_id");
            participant.Property(x => x.MemberId).HasColumnName("member_id");
            participant.HasKey("ChatThreadId", nameof(ChatThreadParticipant.MemberId));
            participant.HasIndex(x => x.MemberId);
        });
        builder.Navigation(x => x.Participants).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Ignore(x => x.DomainEvents);
    }
}
