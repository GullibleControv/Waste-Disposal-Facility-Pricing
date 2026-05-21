namespace PricingSystem.Api.Services;

public interface IDropOffHistoryStore
{
    int GetVisitCountForMonth(string userId, DateOnly date);
    void RecordVisit(string userId, DateOnly date);
}
