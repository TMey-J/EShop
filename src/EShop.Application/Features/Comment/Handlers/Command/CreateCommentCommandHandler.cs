using EShop.Application.Features.Comment.Requests.Command;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.Comment.Handlers.Command;

public class CreateCommentCommandHandler(IProductRepository productRepository,
    ICommentRepository commentRepository,
    IRabbitmqPublisherService rabbitmqPublisher) :
    IRequestHandler<CreateCommentCommandRequest, CreateCommentCommandResponse>
{
    private readonly IProductRepository _product = productRepository;
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IRabbitmqPublisherService _rabbitmqPublisher = rabbitmqPublisher;


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
        await using var transaction = await _commentRepository.BeginTransactionAsync();
        try
        {
            await _commentRepository.CreateAsync(newComment);
            await _commentRepository.SaveChangesAsync();
            var mongoComment = new MongoComment
            {
                ProductId = newComment.ProductId,
                Body = newComment.Body,
                Rating = newComment.Rating,
                IsConfirmed = newComment.IsConfirmed,
                CreateDateTime = newComment.CreateDateTime,
                ModifiedDateTime = newComment.ModifiedDateTime,
                ParentId = newComment.ParentId,
                Id = newComment.Id
            };
            await _rabbitmqPublisher.PublishMessageAsync<MongoComment>(
                new(ActionTypes.Create, mongoComment),
                RabbitmqConstants.QueueNames.Comment,
                RabbitmqConstants.RoutingKeys.Comment);
           await transaction.CommitAsync(cancellationToken);
            return new CreateCommentCommandResponse();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}