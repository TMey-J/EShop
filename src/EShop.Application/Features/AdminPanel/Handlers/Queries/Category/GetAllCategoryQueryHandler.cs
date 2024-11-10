using EShop.Application.Features.AdminPanel.Requests.Queries.Category;

namespace EShop.Application.Features.AdminPanel.Handlers.Queries.Category;

public class GetAllCategoryQueryHandler(ICategoryRepository category):
    IRequestHandler<GetAllCategoryQueryRequest,GetAllCategoryQueryResponse>
{
    private readonly ICategoryRepository _category = category;

    public async Task<GetAllCategoryQueryResponse> Handle(GetAllCategoryQueryRequest request, CancellationToken cancellationToken)
    {
        var categories = await _category.GetAllAsync(request.Search);
        
        return categories;
    }
}