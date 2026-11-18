namespace DomusMind.Application.Features.HouseholdAssets;

public enum AssetsErrorCode { AccessDenied, NotFound, InvalidInput, Conflict }
public sealed class AssetsException(AssetsErrorCode code, string message) : Exception(message)
{
    public AssetsErrorCode Code { get; } = code;
}
