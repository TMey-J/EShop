namespace EShop.Application.Features.AdminPanel.Tag.Handlers.Queries;

public class GetAllTagsQueryHandler(ITagRepository tag):
    IRequestHandler<GetAllTagsQueryRequest,GetAllTagsQueryResponse>
{
    private readonly ITagRepository _tag = tag;

    public async Task<GetAllTagsQueryResponse> Handle(GetAllTagsQueryRequest request, CancellationToken cancellationToken)
    {
        var tags = await _tag.GetAllAsync(request.Search);
        
        return tags;
    }
}