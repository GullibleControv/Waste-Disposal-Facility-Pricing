using System.Text.Json.Serialization;

namespace PricingSystem.Api.Models;

public record CalculatePriceRequest(
    [property: JsonPropertyName("date")] string Date,
    [property: JsonPropertyName("person_id")] string PersonId,
    [property: JsonPropertyName("visit_id")] string VisitId,
    [property: JsonPropertyName("dropped_fractions")] List<DroppedFraction> DroppedFractions
);

public record DroppedFraction(
    [property: JsonPropertyName("fraction_type")] string FractionType,
    [property: JsonPropertyName("amount_dropped")] decimal AmountDropped
);
