namespace DomusMind.Contracts.Family;

/// <summary>Returned once after admin triggers a password regeneration. temporaryPassword is never retrievable again.</summary>
public sealed record RegenerateTemporaryPasswordResponse(string TemporaryPassword, bool MustChangePassword);
