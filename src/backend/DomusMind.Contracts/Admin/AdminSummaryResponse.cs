namespace DomusMind.Contracts.Admin;

public sealed record AdminSummaryResponse(
    string DeploymentMode,
    int HouseholdCount,
    int UserCount,
    int PendingInvitationCount,
    bool IsSystemInitialized);
