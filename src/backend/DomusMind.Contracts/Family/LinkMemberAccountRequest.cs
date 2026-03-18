namespace DomusMind.Contracts.Family;

public sealed record LinkMemberAccountRequest(
    string Username,
    string TemporaryPassword);
