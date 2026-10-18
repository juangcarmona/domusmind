using DomusMind.Domain.Chats;
using DomusMind.Domain.Chats.ValueObjects;
using DomusMind.Domain.Family;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.Chats;

public sealed class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("chat_messages");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => ChatMessageId.From(x)).HasColumnName("id");
        builder.Property(x => x.ChatThreadId).HasColumnName("chat_thread_id");
        builder.Property(x => x.FamilyId).HasConversion(x => x.Value, x => FamilyId.From(x)).HasColumnName("family_id");
        builder.Property(x => x.AuthorMemberId).HasColumnName("author_member_id");
        builder.Property(x => x.AuthorType).HasColumnName("author_type").HasMaxLength(20).IsRequired();
        builder.Property(x => x.Text).HasColumnName("text").HasMaxLength(4000).IsRequired();
        builder.Property(x => x.ReplyToMessageId).HasColumnName("reply_to_message_id");
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc");
        builder.HasIndex(x => new { x.ChatThreadId, x.CreatedAtUtc });
        builder.HasIndex(x => x.FamilyId);
        builder.HasIndex(x => x.ReplyToMessageId).IsUnique();
        builder.Ignore(x => x.DomainEvents);
    }
}
