using DomusMind.Domain.Family;
using DomusMind.Domain.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.Inbox;

public sealed class InboxItemConfiguration : IEntityTypeConfiguration<InboxItem>
{
    public void Configure(EntityTypeBuilder<InboxItem> builder)
    {
        builder.ToTable("inbox_items");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => InboxItemId.From(x)).HasColumnName("id");
        builder.Property(x => x.FamilyId).HasConversion(x => x.Value, x => FamilyId.From(x)).HasColumnName("family_id");
        builder.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id");
        builder.Property(x => x.Version).HasColumnName("version").IsConcurrencyToken();
        builder.Property(x => x.RawText).HasColumnName("raw_text").HasMaxLength(4000).IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc");
        builder.HasIndex(x => new { x.FamilyId, x.CreatedAtUtc });
        builder.OwnsMany(x => x.Proposals, proposal =>
        {
            proposal.ToTable("inbox_proposals");
            proposal.WithOwner().HasForeignKey("inbox_item_id");
            proposal.HasKey(x => x.Id);
            proposal.Property(x => x.Id).ValueGeneratedNever().HasColumnName("id");
            proposal.Property(x => x.Type).HasColumnName("type").HasMaxLength(20).IsRequired();
            proposal.Property(x => x.Title).HasColumnName("title").HasMaxLength(200).IsRequired();
            proposal.Property(x => x.Description).HasColumnName("description").HasMaxLength(2000);
            proposal.Property(x => x.Date).HasColumnName("date");
            proposal.Property(x => x.Time).HasColumnName("time");
            proposal.Property(x => x.EndDate).HasColumnName("end_date");
            proposal.Property(x => x.EndTime).HasColumnName("end_time");
            proposal.Property(x => x.DueDate).HasColumnName("due_date");
            proposal.Property(x => x.Status).HasColumnName("status").HasMaxLength(20).IsRequired();
            proposal.Property(x => x.UnresolvedDetails).HasColumnName("unresolved_details").HasMaxLength(500);
            proposal.Property(x => x.ResultId).HasColumnName("result_id");
            proposal.Property(x => x.TargetPlanTitle).HasColumnName("target_plan_title").HasMaxLength(200);
            proposal.Property(x => x.TargetPlanId).HasColumnName("target_plan_id");
            proposal.Property(x => x.ReminderMinutesBefore).HasColumnName("reminder_minutes_before");
            proposal.HasIndex("inbox_item_id");
        });
        builder.Navigation(x => x.Proposals).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Ignore(x => x.DomainEvents);
    }
}
