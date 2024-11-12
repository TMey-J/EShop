using EShop.Application.Features.AdminPanel.Requests.Queries.Tag;

namespace EShop.Application.Features.AdminPanel.Handlers.Queries.Tag;

public class GetAllTagQueryHandler(ITagRepository tag):
    IRequestHandler<GetAllTagQueryRequest,GetAllTagQueryResponse>
{
    private readonly ITagRepository _tag = tag;

    public async Task<GetAllTagQueryResponse> Handle(GetAllTagQueryRequest request, CancellationToken cancellationToken)
    {
        var tags = await _tag.GetAllAsync(request.Search);
        
        return tags;
    }
}