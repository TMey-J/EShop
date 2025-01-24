using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.Comment.Requests.Queries;

namespace EShop.Application.Features.Comment.Handlers.Queries;

public class GetAllCommentsQueryHandler(IMongoProductRepository productRepository,
    IMongoCommentRepository commentRepository) :
    IRequestHandler<GetAllCommentsQueryRequest, GetAllCommentsQueryResponse>
{
    private readonly IMongoProductRepository _product = productRepository;
    private readonly IMongoCommentRepository _commentRepository = commentRepository;


    public async Task<GetAllCommentsQueryResponse> Handle(GetAllCommentsQueryRequest request, CancellationToken cancellationToken)
    {
        if (!await _product.IsExistByIdAsync(request.ProductId))
        {
            throw new NotFoundException(NameToReplaceInException.Product);
        }
        var comments=await _commentRepository
            .GetAllForProductAsync(request.ProductId,request.Pagination,cancellationToken);
        return comments;
    }
}