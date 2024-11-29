using EShop.Application.Features.AdminPanel.User.Requests.Queries;

namespace EShop.Application.Contracts.MongoDb
{
    public interface IMongoUserRepository:IMongoGenericRepository<User>
    {
        Task<User?> FindByPhoneNumberAsync(string email);
        Task<User?> FindByEmailAsync(string phoneNumber);
        Task <(User?,bool)> FindByEmailOrPhoneNumberWithCheckIsEmailAsync(string emailOrPhoneNumber);
        Task <GetAllUsersQueryResponse> GetAllWithFiltersAsync(SearchUserDto search);
    }
}
