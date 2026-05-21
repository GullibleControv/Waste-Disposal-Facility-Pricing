namespace PricingSystem.Api.Services;

public static class RateTable
{
    private static readonly Dictionary<(string City, string Fraction), decimal> Rates = new()
    {
        { ("Osaka", "Construction waste"), 150m },
        { ("Osaka", "Green waste"),        100m },
        { ("Tokyo", "Construction waste"), 190m },
        { ("Tokyo", "Green waste"),         80m },
    };

    public static decimal GetRate(string city, string fractionType)
    {
        if (!Rates.TryGetValue((city, fractionType), out var rate))
            throw new ArgumentException($"Unknown combination: city='{city}', fraction='{fractionType}'");

        return rate;
    }
}
