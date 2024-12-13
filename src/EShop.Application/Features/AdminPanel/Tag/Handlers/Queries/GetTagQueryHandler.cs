using EShop.Application.Contracts.MongoDb;

namespace EShop.Application.Features.AdminPanel.Tag.Handlers.Queries;

public class GetTagQueryHandler(IMongoTagRepository tag):
    IRequestHandler<GetTagQueryRequest,GetTagQueryResponse>
{
    private readonly IMongoTagRepository _tag = tag;

    public async Task<GetTagQueryResponse> Handle(GetTagQueryRequest request, CancellationToken cancellationToken)
    {
        var category = await _tag.FindByIdAsync(request.Id)??
                       throw new NotFoundException(NameToReplaceInException.Tag);
        
        return new GetTagQueryResponse(category.Id,category.Title);
    }
}