using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;

namespace EShop.Application.Features.AdminPanel.Tag.Handlers.Queries;

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