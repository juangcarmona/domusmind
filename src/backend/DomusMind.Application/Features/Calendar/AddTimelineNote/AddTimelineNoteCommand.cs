using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Calendar;

namespace DomusMind.Application.Features.Calendar.AddTimelineNote;

public sealed record AddTimelineNoteCommand(Guid FamilyId, string Text, DateOnly Date, Guid RequestedByUserId)
    : ICommand<TimelineNoteItem>;
