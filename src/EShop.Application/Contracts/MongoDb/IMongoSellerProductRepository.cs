using EShop.Application.Features.SellerPanel.Requests.Queries;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Contracts.MongoDb
{
    public interface IMongoSellerProductRepository
    {
        
        Task CreateAsync(MongoSellerProduct entity);   
        Task CreateAllAsync(List<MongoSellerProduct> entities);   
        Task Update(MongoSellerProduct entity);
        Task Delete(MongoSellerProduct entity);
        Task<IEnumerable<MongoSellerProduct>> GetAllBySellerIdAsync(long sellerId);
        Task<IEnumerable<MongoSellerProduct>> GetAllByProductIdAsync(long productId);
        Task<MongoSellerProduct?> FindReserveAsync(long productId,long colorId,long sellerId);

        Task<GetAllReservedProductsQueryResponse> GetAllReservedProductsAsync(SearchSellerProductDto search,
            long sellerId);
    }
}
