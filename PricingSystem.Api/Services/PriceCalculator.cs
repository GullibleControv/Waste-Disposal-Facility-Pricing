using PricingSystem.Api.Models;

namespace PricingSystem.Api.Services;

public record PricingPolicy(
    int SurchargeVisitThreshold = 3,
    decimal SurchargePercentage = 5.0m,
    string Currency = "JPY"
);

public class PriceCalculator
{
    private readonly IUserRepository _users;
    private readonly IDropOffHistoryStore _history;
    private readonly PricingPolicy _policy;

    public PriceCalculator(IUserRepository users, IDropOffHistoryStore history, PricingPolicy policy)
    {
        _users = users;
        _history = history;
        _policy = policy;
    }

    public CalculatePriceResponse Calculate(CalculatePriceRequest request)
    {
        // Step 1 — Validate inputs before touching any state
        ValidateRequest(request);

        // Step 2 — Look up the user (city determines which rate table row to use)
        var user = _users.GetUser(request.PersonId)
            ?? throw new ArgumentException($"User not found: '{request.PersonId}'");

        // Step 3 — Calculate base price (RateTable also rejects unknown fraction types here)
        var date = DateOnly.Parse(request.Date);
        decimal basePrice = 0;

        foreach (var fraction in request.DroppedFractions)
        {
            var rate = RateTable.GetRate(user.City, fraction.FractionType);
            basePrice += rate * fraction.AmountDropped;
        }

        // Step 4 — Read count first, then record. Order matters: read before write.
        var existingCount = _history.GetVisitCountForMonth(request.PersonId, date);
        _history.RecordVisit(request.PersonId, date);
        var currentVisitNumber = existingCount + 1;

        // Step 5 — Apply surcharge if this is the threshold visit or beyond
        var finalPrice = currentVisitNumber >= _policy.SurchargeVisitThreshold
            ? ApplySurcharge(basePrice, _policy.SurchargePercentage)
            : basePrice;

        return new CalculatePriceResponse(finalPrice, _policy.Currency, request.VisitId, request.PersonId);
    }

    private void ValidateRequest(CalculatePriceRequest request)
    {
        foreach (var fraction in request.DroppedFractions)
        {
            if (fraction.AmountDropped <= 0)
                throw new ArgumentException(
                    $"Amount must be positive. Got {fraction.AmountDropped} for '{fraction.FractionType}'.");
        }
    }

    private static decimal ApplySurcharge(decimal basePrice, decimal percentage)
    {
        var multiplier = 1 + (percentage / 100m);
        return Math.Round(basePrice * multiplier, 0, MidpointRounding.AwayFromZero);
    }
}
