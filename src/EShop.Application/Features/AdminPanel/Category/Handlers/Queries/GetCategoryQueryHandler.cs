using EShop.Application.Contracts.MongoDb;

namespace EShop.Application.Features.AdminPanel.Category.Handlers.Queries;

public class GetCategoryQueryHandler(IMongoCategoryRepository category):
    IRequestHandler<GetCategoryQueryRequest,GetCategoryQueryResponse>
{
    private readonly IMongoCategoryRepository _category = category;

    public async Task<GetCategoryQueryResponse> Handle(GetCategoryQueryRequest request, CancellationToken cancellationToken)
    {
        var category = await _category.FindByIdAsync(request.Id)??
                       throw new NotFoundException(NameToReplaceInException.Category);
        
        return new GetCategoryQueryResponse(category.Id,category.Title,category.ParentId);
    }
}