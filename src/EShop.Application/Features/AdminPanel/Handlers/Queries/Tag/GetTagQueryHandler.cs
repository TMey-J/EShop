using EShop.Application.Features.AdminPanel.Requests.Queries.Category;
using EShop.Application.Features.AdminPanel.Requests.Queries.Tag;

namespace EShop.Application.Features.AdminPanel.Handlers.Queries.Tag;

public class GetTagQueryHandler(ITagRepository tag):
    IRequestHandler<GetTagQueryRequest,GetTagQueryResponse>
{
    private readonly ITagRepository _tag = tag;

    public async Task<GetTagQueryResponse> Handle(GetTagQueryRequest request, CancellationToken cancellationToken)
    {
        var category = await _tag.FindByIdAsync(request.Id)??
                       throw new NotFoundException(NameToReplaceInException.Tag);
        
        return new GetTagQueryResponse(category.Id,category.Title);
    }
}