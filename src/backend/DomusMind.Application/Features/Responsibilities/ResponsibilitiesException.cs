namespace DomusMind.Application.Features.Responsibilities;

public enum ResponsibilitiesErrorCode
{
    ResponsibilityDomainNotFound,
    AccessDenied,
    MemberNotFound,
    DuplicateSecondaryOwner,
    InvalidInput,
}

public sealed class ResponsibilitiesException : Exception
{
    public ResponsibilitiesErrorCode Code { get; }

    public ResponsibilitiesException(ResponsibilitiesErrorCode code, string message) : base(message)
    {
        Code = code;
    }
}
