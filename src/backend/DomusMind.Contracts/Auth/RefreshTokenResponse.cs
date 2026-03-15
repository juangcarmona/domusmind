namespace DomusMind.Contracts.Auth;

public sealed record RefreshTokenResponse(string AccessToken, string RefreshToken);
