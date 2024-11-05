using EShop.Application.Features.AdminPanel.Requests.Queries.Category;

namespace EShop.Application.Features.AdminPanel.Handlers.Queries.Category;

public class GetAllCategoryQueryHandler(ICategoryRepository category):
    IRequestHandler<GetAllCategoryQueryRequest,List<GetAllCategoryQueryResponse>>
{
    private readonly ICategoryRepository _category = category;

    public async Task<List<GetAllCategoryQueryResponse>> Handle(GetAllCategoryQueryRequest request, CancellationToken cancellationToken)
    {
        var categories = await _category.GetAllAsync();
        
        var formattedCategories=categories.Select(x =>
             new GetAllCategoryQueryResponse(x.Id, x.Title,
                 _category.GetParentIdWithHierarchyIdAsync(x.Parent).Result ?? null, x.Picture)).ToList();
        
        return formattedCategories;
    }
}