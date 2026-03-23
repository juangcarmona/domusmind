namespace DomusMind.Application.Abstractions.Security;

/// <summary>
/// Generates short, human-shareable temporary passwords for household member provisioning.
/// </summary>
public interface ITemporaryPasswordGenerator
{
    /// <summary>
    /// Generates a temporary password that is:
    /// - 8–10 characters long
    /// - Mixed upper-case, lower-case and digits
    /// - Free of visually ambiguous characters (0, O, I, l, 1)
    /// The plain-text value is returned only here and must never be stored or logged.
    /// </summary>
    string Generate();
}
