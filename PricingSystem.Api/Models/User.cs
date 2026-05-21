using System.Text.Json.Serialization;

namespace PricingSystem.Api.Models;

public record User(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("city")] string City
);
