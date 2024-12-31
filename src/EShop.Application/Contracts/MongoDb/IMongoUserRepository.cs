using EShop.Application.Features.AdminPanel.User.Requests.Queries;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Contracts.MongoDb
{
    public interface IMongoUserRepository:IMongoGenericRepository<MongoUser>
    {
        Task<MongoUser?> FindByPhoneNumberAsync(string email);
        Task<MongoUser?> FindByEmailAsync(string phoneNumber);
        Task <(MongoUser?,bool)> FindByEmailOrPhoneNumberWithCheckIsEmailAsync(string emailOrPhoneNumber);
        Task <GetAllUsersQueryResponse> GetAllWithFiltersAsync(SearchUserDto search);
    }
}
