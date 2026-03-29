namespace DomusMind.Contracts.Admin;

public sealed record DisableUserResponse(Guid UserId, bool IsDisabled);

public sealed record EnableUserResponse(Guid UserId, bool IsDisabled);
