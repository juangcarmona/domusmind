namespace DomusMind.Contracts.Calendar;

public sealed record ConnectOutlookAccountRequest(
    string AuthorizationCode,
    string State,
    string RedirectUri,
    string? AccountDisplayLabel,
    string? ConnectState);
