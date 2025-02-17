using EShop.Application.Features.SellerPanel.Requests.Commands;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.SellerPanel.Handlers.Commands;

public class ReserveProductCommandHandler(
    IProductRepository productRepository,
    IColorRepository colorRepository,
    ISellerProductRepository sellerProductRepository,
    ISellerRepository sellerRepository,
    IRabbitmqPublisherService rabbitmqPublisher)
    : IRequestHandler<ReserveProductCommandRequest, ReserveProductCommandResponse>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IColorRepository _colorRepository = colorRepository;
    private readonly ISellerProductRepository _sellerProductRepository = sellerProductRepository;
    private readonly ISellerRepository _sellerRepository = sellerRepository;
    private readonly IRabbitmqPublisherService _rabbitmqPublisher = rabbitmqPublisher;

    public async Task<ReserveProductCommandResponse> Handle(ReserveProductCommandRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByIdAsync(request.ProductId) ??
                      throw new CustomBadRequestException(["محصول موردنظر یافت نشد"]);
        var images = await _productRepository.GetImagesByProductIdAsync(product.Id);
        var color = await _colorRepository.FindByAsync(nameof(Color.Code), request.ColorCode)
                    ?? throw new NotFoundException(NameToReplaceInException.Color);

        if (await _sellerProductRepository.IsExistAsync(request.SellerId, request.ProductId, color.Id))
        {
            throw new CustomBadRequestException(["این محصول با این مشخصات قبلا توسط شما رزرو شده است"]);
        }

        var sellerProduct = new SellerProduct()
        {
            ProductId = request.ProductId,
            Count = request.Count,
            BasePrice = request.BasePrice,
            SellerId = request.SellerId,
            ColorId = color.Id,
            DiscountPercentage = request.DiscountPercentage,
            EndOfDiscount = request.EndOfDiscount
        };
        await using var transaction = await _sellerRepository.BeginTransactionAsync();
        try
        {
            await _sellerProductRepository.ReserveProductAsync(sellerProduct);
            await _sellerProductRepository.SaveChangesAsync();
            var mongoSellerProduct = new MongoSellerProduct
            {
                Id = $"{request.ProductId}{request.SellerId}{request.ColorCode}",
                ProductId = request.ProductId,
                Count = request.Count,
                BasePrice = request.BasePrice,
                SellerId = request.SellerId,
                ColorId = color.Id,
                DiscountPercentage = request.DiscountPercentage,
                EndOfDiscount = request.EndOfDiscount,
                Product = new CustomMongoProduct()
                {
                    Title = product.Title,
                    EnglishTitle = product.EnglishTitle,
                    Images = images.Select(x => x.ImageName).ToList(),
                    CategoryId = product.CategoryId
                }
            };
            await _rabbitmqPublisher.PublishMessageAsync(
                new MessageModel<MongoSellerProduct>(ActionTypes.Create, mongoSellerProduct),
                RabbitmqConstants.QueueNames.SellerProduct,
                RabbitmqConstants.RoutingKeys.SellerProduct);
            await transaction.CommitAsync(cancellationToken);
            return new ReserveProductCommandResponse();
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}