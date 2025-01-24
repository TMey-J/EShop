using EShop.Application.Features.Comment.Requests.Command;

namespace EShop.Application.Features.Comment.Handlers.Command;

public class CreateCommentCommandHandler(IProductRepository productRepository,
    ICommentRepository commentRepository) :
    IRequestHandler<CreateCommentCommandRequest, CreateCommentCommandResponse>
{
    private readonly IProductRepository _product = productRepository;
    private readonly ICommentRepository _commentRepository = commentRepository;


    public async Task<CreateCommentCommandResponse> Handle(CreateCommentCommandRequest request, CancellationToken cancellationToken)
    {
        if (!await _product.IsExistsByIdAsync(request.ProductId))
        {
            throw new NotFoundException(NameToReplaceInException.Product);
        }

        var newComment = new Domain.Entities.Comment
        {
            ProductId = request.ProductId,
            Body = request.Body,
            Rating = request.Rating,
            IsConfirmed = false,
            CreateDateTime = DateTime.Now,
            ModifiedDateTime = DateTime.Now
        };
        if (request.ReplayId != null)
        {
            var comment=await _commentRepository.FindByIdAsync(request.ReplayId??0)
                ?? throw new CustomBadRequestException(["کامنت مورد نظر شما یافت نشد"]);
            if (!await _commentRepository.IsReplyValid(comment.ProductId, request.ReplayId ?? 0))
            {
                throw new CustomBadRequestException(["کامت مورد پاسخ شما در این محصول وجود ندارد"]);
            }
            newComment.ParentId = request.ReplayId;
        }
        await _commentRepository.CreateAsync(newComment);
        await _commentRepository.SaveChangesAsync();
        return new CreateCommentCommandResponse();
    }
}