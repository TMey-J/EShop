﻿using Blogger.Application.Common.Exceptions;
using EShop.Application.Constants;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Category.Requests.Queries;
using EShop.Domain.Entities.Mongodb;
using EShop.Infrastructure.Databases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EShop.Infrastructure.Repositories.MongoDb
{
    public class MongoCategoryRepository(MongoDbContext mongoDb)
        : MongoGenericRepository<MongoCategory>(mongoDb,MongoCollectionsName.Category), IMongoCategoryRepository
    {
        private readonly IMongoCollection<MongoCategory> _category = mongoDb.GetCollection<MongoCategory>(MongoCollectionsName.Category);
        private readonly IMongoCollection<CategoryFeature> _categoryFeature = mongoDb.GetCollection<CategoryFeature>(MongoCollectionsName.CategoryFeature);
        private readonly IMongoCollection<MongoFeature> _feature = mongoDb.GetCollection<MongoFeature>(MongoCollectionsName.Feature);

        public async Task<GetAllCategoryQueryResponse> GetAllAsync(SearchCategoryDto search)
        {
            var category = _category.AsQueryable().IgnoreQueryFilters();

            #region Search

            category = category.CreateContainsExpression(nameof(Category.Title), search.Title);

            #endregion

            #region Sort

            category = category.CreateOrderByExpression(search.SortingBy.ToString(), search.SortingAs);

            category = category.CreateDeleteStatusExpression(nameof(BaseEntity.IsDelete), search.DeleteStatus);

            #endregion

            #region Paging

            (IQueryable<MongoCategory> query, int pageCount) pagination =
                category.Page(search.Pagination.CurrentPage, search.Pagination.TakeRecord);
            category = pagination.query;

            #endregion

            var categories = await MongoQueryable.ToListAsync(category.Select
            (x => new ShowCategoryDto(x.Id, x.Title,
                x.ParentId,
                x.Picture)));

            return new GetAllCategoryQueryResponse(categories, search, pagination.pageCount);
        }

        public async Task<List<MongoFeature>> GetCategoryFeatures(long categoryId)
        {
            var featuresIQueryable = from categoryFeature in _categoryFeature.AsQueryable()
                    .Where(x => x.CategoryId == categoryId)
                join feature in _feature on categoryFeature.FeatureId equals feature.Id
                select new MongoFeature()
                {
                    Id = feature.Id,
                    Name = feature.Name,
                    IsDelete = feature.IsDelete
                };
            return await MongoQueryable.ToListAsync(featuresIQueryable);
        }

        public async Task<List<string>> GetCategoryHierarchyAsync(long categoryId)
        {
            var categories= new List<string>();
            var category=await _category.Find(x=>x.Id==categoryId).SingleOrDefaultAsync()
                ?? throw new CustomInternalServerException([$"Category with id {categoryId} not found"]);
            categories.Add(category.Title);
            while (category.ParentId!=null)
            {
                category=await _category.Find(x=>x.Id==category.ParentId).SingleOrDefaultAsync()
                         ?? throw new CustomInternalServerException([$"Category with id {category.ParentId} not found"]);
                categories.Add(category.Title);
            }

            categories.Reverse();
            return categories;
        }
    }
}