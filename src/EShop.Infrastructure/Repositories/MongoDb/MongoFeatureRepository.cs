using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Feature.Requests.Queries;
using EShop.Domain.Entities.Mongodb;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EShop.Infrastructure.Repositories.MongoDb;

    public class MongoFeatureRepository(MongoDbContext mongoDb) : MongoGenericRepository<MongoFeature>(mongoDb,MongoCollectionsName.Feature), IMongoFeatureRepository
    {
        private readonly IMongoCollection<MongoFeature> _feature = mongoDb.GetCollection<MongoFeature>(MongoCollectionsName.Feature);
        public async Task<GetAllFeaturesQueryResponse> GetAllAsync(SearchFeatureDto search)
        {
            var featureQuery = _feature.AsQueryable().IgnoreQueryFilters();

            #region Search

            featureQuery = featureQuery.CreateContainsExpression(nameof(Feature.Name), search.Name);

            #endregion

            #region Sort

            featureQuery = featureQuery.CreateOrderByExpression(search.SortingBy.ToString(), search.SortingAs);

            featureQuery = featureQuery.CreateDeleteStatusExpression(nameof(BaseEntity.IsDelete), search.DeleteStatus);

            #endregion

            #region Paging

            (IQueryable<MongoFeature> query, int pageCount) pagination =
                featureQuery.Page(search.Pagination.CurrentPage, search.Pagination.TakeRecord);
            featureQuery = pagination.query;

            #endregion

            var features = await MongoQueryable.ToListAsync(featureQuery.Select
                (x => new ShowFeatureDto(x.Id, x.Name)));

            return new GetAllFeaturesQueryResponse(features, search, pagination.pageCount);
        }
    }
