using EShop.Application.Features.AdminPanel.Seller.Requests.Queries;

namespace EShop.Application.Contracts.MongoDb
{
    public interface IMongoSellerRepository:IMongoGenericRepository<Seller>
    {
        Task<GetAllSellersQueryResponse> GetAllAsync(SearchSellerDto search);
    }
}
