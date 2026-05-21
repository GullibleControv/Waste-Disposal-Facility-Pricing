using System.Text.Json;
using PricingSystem.Api.Models;

namespace PricingSystem.Api.Services;

public class JsonUserRepository : IUserRepository
{
    private readonly Dictionary<string, User> _users;

    public JsonUserRepository(string filePath)
    {
        var json = File.ReadAllText(filePath);
        var list = JsonSerializer.Deserialize<List<User>>(json)
            ?? throw new InvalidOperationException("users.json is empty or invalid.");

        _users = list.ToDictionary(u => u.Id);
    }

    public User? GetUser(string userId)
        => _users.TryGetValue(userId, out var user) ? user : null;
}
