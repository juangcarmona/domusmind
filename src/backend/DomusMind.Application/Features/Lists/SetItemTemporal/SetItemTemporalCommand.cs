using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Lists;

namespace DomusMind.Application.Features.Lists.SetItemTemporal;

public sealed record SetItemTemporalCommand(
    Guid ListId,
    Guid ItemId,
    DateOnly? DueDate,
    DateTimeOffset? Reminder,
    string? Repeat,
    Guid RequestedByUserId,
    bool ClearDueDate = false,
    bool ClearReminder = false,
    bool ClearRepeat = false) : ICommand<SetItemTemporalResponse>;
