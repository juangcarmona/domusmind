using DomusMind.Domain.Calendar;
using DomusMind.Domain.Family;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomusMind.Infrastructure.Persistence.Configurations.Calendar;

public sealed class TimelineNoteConfiguration : IEntityTypeConfiguration<TimelineNote>
{
    public void Configure(EntityTypeBuilder<TimelineNote> builder)
    {
        builder.ToTable("calendar_timeline_notes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => TimelineNoteId.From(x))
            .HasColumnName("id").ValueGeneratedNever();
        builder.Property(x => x.FamilyId).HasConversion(x => x.Value, x => FamilyId.From(x))
            .HasColumnName("family_id").IsRequired();
        builder.Property(x => x.Date).HasColumnName("date").IsRequired();
        builder.Property(x => x.Text).HasColumnName("text").HasMaxLength(4000).IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.HasIndex(x => new { x.FamilyId, x.Date });
        builder.Ignore(x => x.DomainEvents);
    }
}
