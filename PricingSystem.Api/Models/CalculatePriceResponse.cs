using System.Text.Json.Serialization;

namespace PricingSystem.Api.Models;

public record CalculatePriceResponse(
    [property: JsonPropertyName("price_amount")] decimal PriceAmount,
    [property: JsonPropertyName("price_currency")] string PriceCurrency,
    [property: JsonPropertyName("visit_id")] string VisitId,
    [property: JsonPropertyName("person_id")] string PersonId
);
