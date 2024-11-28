using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;

namespace EShop.Application.Features.AdminPanel.Tag.Handlers.Queries;

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