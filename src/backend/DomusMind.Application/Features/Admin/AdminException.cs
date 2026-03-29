namespace DomusMind.Application.Features.Admin;

public enum AdminErrorCode
{
    UserNotFound,
    InvitationNotFound,
    InvitationNotRevocable,
    InvalidInput,
    CannotDisableOperator,
}

public sealed class AdminException : Exception
{
    public AdminErrorCode Code { get; }

    public AdminException(AdminErrorCode code, string message)
        : base(message)
    {
        Code = code;
    }
}
