using EShop.Application.Features.AdminPanel.Product.Requests.Queries;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Contracts.MongoDb
{
    public interface IMongoProductRepository:IMongoGenericRepository<MongoProduct>
    {
        Task<GetAllProductQueryResponse> GetAllAsync(SearchProductDto search);
        Task<int> CountProductByIdAsync(long productId);

    }
}
