using PricingSystem.Api.Models;

namespace PricingSystem.Api.Services;

public interface IUserRepository
{
    User? GetUser(string userId);
}
