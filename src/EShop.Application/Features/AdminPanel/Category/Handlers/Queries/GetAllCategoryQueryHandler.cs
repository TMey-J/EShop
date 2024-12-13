using EShop.Application.Contracts.MongoDb;

namespace EShop.Application.Features.AdminPanel.Category.Handlers.Queries;

public class GetAllCategoryQueryHandler(IMongoCategoryRepository category):
    IRequestHandler<GetAllCategoryQueryRequest,GetAllCategoryQueryResponse>
{
    private readonly IMongoCategoryRepository _category = category;

    public async Task<GetAllCategoryQueryResponse> Handle(GetAllCategoryQueryRequest request, CancellationToken cancellationToken)
    {
        if (request.Search is null)
        {
            var categories = await _category.GetAllAsync();
            var showCategory = categories
                .Select(x => new ShowCategoryDto(x.Id, x.Title, x.ParentId, x.Picture)).ToList();
            return new(showCategory,null,null);
        }
        var categoriesWithSearch = await _category.GetAllAsync(request.Search);
        
        return categoriesWithSearch;
    }
}