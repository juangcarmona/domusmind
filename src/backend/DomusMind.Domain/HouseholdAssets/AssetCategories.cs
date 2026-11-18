namespace DomusMind.Domain.HouseholdAssets;

public static class AssetCategories
{
    public static IReadOnlyList<string> All { get; } = Array.AsReadOnly(new[]
    {
        "Property", "Vehicle", "Appliance", "Equipment", "Other"
    });

    public static string Require(string category)
    {
        var value = category?.Trim();
        return All.Contains(value, StringComparer.Ordinal)
            ? value!
            : throw new ArgumentException("Choose a supported asset category.", nameof(category));
    }
}
