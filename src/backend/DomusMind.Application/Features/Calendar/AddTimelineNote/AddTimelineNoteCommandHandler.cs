using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Calendar;
using DomusMind.Domain.Calendar;
using DomusMind.Domain.Family;

namespace DomusMind.Application.Features.Calendar.AddTimelineNote;

public sealed class AddTimelineNoteCommandHandler(IDomusMindDbContext db, IEventLogWriter events,
    IFamilyAuthorizationService auth) : ICommandHandler<AddTimelineNoteCommand, TimelineNoteItem>
{
    public async Task<TimelineNoteItem> Handle(AddTimelineNoteCommand command, CancellationToken cancellationToken)
    {
        if (!await auth.CanAccessFamilyAsync(command.RequestedByUserId, command.FamilyId, cancellationToken))
            throw new UnauthorizedAccessException();
        var note = TimelineNote.Create(TimelineNoteId.New(), FamilyId.From(command.FamilyId), command.Date, command.Text);
        db.Set<TimelineNote>().Add(note);
        await events.WriteAsync([note], cancellationToken);
        note.ClearDomainEvents();
        return new TimelineNoteItem(note.Id.Value, note.Text, note.Date.ToString("yyyy-MM-dd"), note.CreatedAtUtc);
    }
}
