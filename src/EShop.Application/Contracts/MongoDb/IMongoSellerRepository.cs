using EShop.Application.Features.AdminPanel.Seller.Requests.Queries;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Contracts.MongoDb
{
    public interface IMongoSellerRepository:IMongoGenericRepository<MongoSeller>
    {
        Task<GetAllSellersQueryResponse> GetAllAsync(SearchSellerDto search);
    }
}
