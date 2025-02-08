using EShop.Application.Features.SellerPanel.Requests.Commands;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.SellerPanel.Handlers.Commands;

public class UpdateReservedProductCommandHandler(
    IProductRepository productRepository,
    IColorRepository colorRepository,
    ISellerProductRepository sellerProductRepository,
    ISellerRepository sellerRepository,
    IRabbitmqPublisherService rabbitmqPublisher)
    : IRequestHandler<UpdateReservedProductCommandRequest, UpdateReservedProductCommandResponse>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IColorRepository _colorRepository = colorRepository;
    private readonly ISellerProductRepository _sellerProductRepository = sellerProductRepository;
    private readonly ISellerRepository _sellerRepository = sellerRepository;
    private readonly IRabbitmqPublisherService _rabbitmqPublisher = rabbitmqPublisher;

    public async Task<UpdateReservedProductCommandResponse> Handle(UpdateReservedProductCommandRequest request,
        CancellationToken cancellationToken)
    {
        var sellerProduct =
            await _sellerProductRepository.FindReserveAsync(request.SellerId, request.ProductId, request.ColorId)
            ?? throw new NotFoundException("محصول شما");
        
        sellerProduct.Count = request.Count;
        sellerProduct.BasePrice = request.BasePrice;
        sellerProduct.DiscountPercentage = request.DiscountPercentage;
        sellerProduct.EndOfDiscount = request.EndOfDiscount;
        await using var transaction = await _sellerRepository.BeginTransactionAsync();
        try
        {
            _sellerProductRepository.UpdateReserveProductAsync(sellerProduct);
            await _sellerProductRepository.SaveChangesAsync();
            
            var mongoSellerProduct = new MongoSellerProduct
            {
                Id = $"{request.ProductId}{request.SellerId}{sellerProduct.Color.Code}",
                ProductId = request.ProductId,
                Count = request.Count,
                BasePrice = request.BasePrice,
                SellerId = request.SellerId,
                ColorId = request.ColorId,
                DiscountPercentage = request.DiscountPercentage,
                EndOfDiscount = request.EndOfDiscount,
                Product = new CustomMongoProduct
                {
                    Title = sellerProduct.Product.Title,
                    EnglishTitle = sellerProduct.Product.EnglishTitle,
                    Images = sellerProduct.Product.Images.Select(x=>x.ImageName).ToList(),
                    CategoryId = sellerProduct.Product.CategoryId
                }
                
            };
            await _rabbitmqPublisher.PublishMessageAsync(
                new MessageModel<MongoSellerProduct>(ActionTypes.Update, mongoSellerProduct),
                RabbitmqConstants.QueueNames.SellerProduct,
                RabbitmqConstants.RoutingKeys.SellerProduct);
            await transaction.CommitAsync(cancellationToken);
            return new UpdateReservedProductCommandResponse();
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}