namespace EShop.Application.Features.AdminPanel.Category.Handlers.Queries;

public class GetCategoryQueryHandler(ICategoryRepository category):
    IRequestHandler<GetCategoryQueryRequest,GetCategoryQueryResponse>
{
    private readonly ICategoryRepository _category = category;

    public async Task<GetCategoryQueryResponse> Handle(GetCategoryQueryRequest request, CancellationToken cancellationToken)
    {
        var category = await _category.FindByIdAsync(request.Id)??
                       throw new NotFoundException(NameToReplaceInException.Category);
        
        var parentId=await _category.GetParentIdWithHierarchyIdAsync(category.Parent);
        
        return new GetCategoryQueryResponse(category.Id,category.Title,parentId);
    }
}