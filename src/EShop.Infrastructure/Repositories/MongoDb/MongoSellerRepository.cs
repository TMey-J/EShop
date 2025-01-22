using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Seller.Requests.Queries;
using EShop.Domain.Entities.Mongodb;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoSellerRepository(MongoDbContext mongoDb)
        : MongoGenericRepository<MongoSeller>(mongoDb,MongoCollectionsName.Seller), IMongoSellerRepository
    {
        private readonly IMongoCollection<MongoSeller> _seller = mongoDb.GetCollection<MongoSeller>(MongoCollectionsName.Seller);

        public async Task<GetAllSellersQueryResponse> GetAllAsync(SearchSellerDto search)
        {
            var sellerQuery = _seller.AsQueryable().IgnoreQueryFilters();

            #region Search

            sellerQuery = sellerQuery.CreateContainsExpression(nameof(Seller.UserName), search.UserName);
            sellerQuery = sellerQuery.CreateContainsExpression(nameof(Seller.ShopName), search.ShopName);
            sellerQuery = sellerQuery.CreateContainsExpression(nameof(Seller.City.Title), search.City);
            sellerQuery = sellerQuery.CreateContainsExpression(nameof(Seller.City.Province.Title), search.Province);

            #endregion

            #region Sort

            sellerQuery = sellerQuery.CreateOrderByExpression(search.SortingBy.ToString(), search.SortingAs);

            sellerQuery = sellerQuery.CreateDeleteStatusExpression(nameof(BaseEntity.IsDelete), search.DeleteStatus);
            sellerQuery = search.ActivationStatus switch
            {
                ActivationStatus.OnlyActive => sellerQuery.Where(x => x.IsActive),
                ActivationStatus.False => sellerQuery.Where(x => !x.IsActive),
                _ => sellerQuery
            };

            #endregion

            #region Paging

            (IQueryable<MongoSeller> query, int pageCount) pagination =
                sellerQuery.Page(search.Pagination.CurrentPage, search.Pagination.TakeRecord);
            sellerQuery = pagination.query;

            #endregion
            var sellers = await MongoQueryable.ToListAsync(sellerQuery.Select
            (x => new ShowSellerDto(x.Id,
                x.UserId,
                x.UserName,
                x.IsLegalPerson,
                x.ShopName,
                x.Logo,
                x.Website, 
                x.City.Title, 
                x.City.Province.Title,
                x.PostalCode,
                x.Address,
                x.RejectReason,
                x.CreatedDateTime,
                x.DocumentStatus,
                x.LegalSeller != null
                    ? new LegalSellerDto()
                    {
                        CompanyName = x.LegalSeller.CompanyName,
                        RegisterNumber = x.LegalSeller.RegisterNumber,
                        EconomicCode = x.LegalSeller.EconomicCode,
                        SignatureOwners = x.LegalSeller.SignatureOwners,
                        ShabaNumber = x.LegalSeller.ShabaNumber,
                        CompanyType = x.LegalSeller.CompanyType
                    }
                    : null,
                x.IndividualSeller != null
                    ? new IndividualSellerDto()
                    {
                        NationalId = x.IndividualSeller.NationalId,
                        CartOrShebaNumber = x.IndividualSeller.CartOrShebaNumber,
                        AboutSeller = x.IndividualSeller.AboutSeller
                    }
                    : null)));

            return new GetAllSellersQueryResponse(sellers, search, pagination.pageCount);
        }
    }
}