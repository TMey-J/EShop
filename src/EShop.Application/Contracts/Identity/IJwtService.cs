using EShop.Domain.Entities.Identity;

namespace Restaurant.Application.Contracts.Identity;

public interface IJwtService
{
    Task<string> Generate(User user);
}