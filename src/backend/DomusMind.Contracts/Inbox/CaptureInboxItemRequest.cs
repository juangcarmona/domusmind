namespace DomusMind.Contracts.Inbox;

public sealed record CaptureInboxItemRequest(Guid FamilyId, string Text);
