namespace PricingSystem.Api.Services;

public class DropOffHistoryStore : IDropOffHistoryStore
{
    private readonly Dictionary<(string UserId, int Year, int Month), int> _counts = new();

    public int GetVisitCountForMonth(string userId, DateOnly date)
    {
        _counts.TryGetValue((userId, date.Year, date.Month), out var count);
        return count;
    }

    public void RecordVisit(string userId, DateOnly date)
    {
        var key = (userId, date.Year, date.Month);
        _counts.TryGetValue(key, out var current);
        _counts[key] = current + 1;
    }
}
