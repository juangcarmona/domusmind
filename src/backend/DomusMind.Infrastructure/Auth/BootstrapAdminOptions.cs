namespace DomusMind.Infrastructure.Auth;

public sealed class BootstrapAdminOptions
{
    public const string SectionName = "BootstrapAdmin";

    public bool Enabled { get; init; }

    public string Email { get; init; } = default!;

    public string Password { get; init; } = default!;

    public string DisplayName { get; init; } = "DomusMind Admin";
}
