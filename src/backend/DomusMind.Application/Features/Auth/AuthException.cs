namespace DomusMind.Application.Auth;

public enum AuthErrorCode
{
    EmailAlreadyTaken,
    InvalidCredentials,
    InvalidRefreshToken,
    InvalidCurrentPassword,
    UserNotFound,
    WeakPassword,
    SamePassword,
}

public sealed class AuthException : Exception
{
    public AuthErrorCode Code { get; }

    public AuthException(AuthErrorCode code, string message)
        : base(message)
    {
        Code = code;
    }
}
