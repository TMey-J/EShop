using EShop.Application.Features.AdminPanel.Requests.Queries.Category;

namespace EShop.Application.Features.AdminPanel.Handlers.Queries.Category;

public class GetCategoryQueryHandler(ICategoryRepository category):
    IRequestHandler<GetCategoryQueryRequest,GetCategoryQueryResponse>
{
    private readonly ICategoryRepository _category = category;

    public async Task<GetCategoryQueryResponse> Handle(GetCategoryQueryRequest request, CancellationToken cancellationToken)
    {
        var category = await _category.FindByIdAsync(request.Id)??
                       throw new NotFoundException("دسته بندی");
        
        var parentId=await _category.GetParentIdWithHierarchyIdAsync(category.Parent);
        
        return new(category.Id,category.Title,parentId);
    }
}