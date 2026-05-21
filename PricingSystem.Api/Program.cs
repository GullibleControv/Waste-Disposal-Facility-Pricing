using PricingSystem.Api.Models;
using PricingSystem.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

// Resolve users.json from the output directory so it works both in dev and after publish
var usersFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "users.json");

// Singletons: these must survive across all requests
builder.Services.AddSingleton<IUserRepository>(_ => new JsonUserRepository(usersFilePath));
builder.Services.AddSingleton<IDropOffHistoryStore, DropOffHistoryStore>();
builder.Services.AddSingleton(new PricingPolicy());

// Scoped: stateless, created fresh per request
builder.Services.AddScoped<PriceCalculator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

// GET / — health check
app.MapGet("/", () => Results.Ok(new { status = "OK" }));

// POST /calculatePrice — calculate price for one drop-off
app.MapPost("/calculatePrice", (CalculatePriceRequest request, PriceCalculator calculator) =>
{
    try
    {
        var result = calculator.Calculate(request);
        return Results.Ok(result);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();
