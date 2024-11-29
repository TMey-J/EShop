using EShop.Application.Contracts.MongoDb;

namespace EShop.Application.Features.AdminPanel.Tag.Handlers.Queries;

public class GetAllTagsQueryHandler(IMongoTagRepository tag):
    IRequestHandler<GetAllTagsQueryRequest,GetAllTagsQueryResponse>
{
    private readonly IMongoTagRepository _tag = tag;

    public async Task<GetAllTagsQueryResponse> Handle(GetAllTagsQueryRequest request, CancellationToken cancellationToken)
    {
        var tags = await _tag.GetAllAsync(request.Search);
        
        return tags;
    }
}