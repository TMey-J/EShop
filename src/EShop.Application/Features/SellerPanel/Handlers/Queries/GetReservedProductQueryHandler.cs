using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.SellerPanel.Requests.Queries;

namespace EShop.Application.Features.SellerPanel.Handlers.Queries;

public class GetReservedProductQueryHandler(
    IMongoSellerProductRepository sellerProductRepository,
    IMongoColorRepository colorRepository)
    : IRequestHandler<GetReservedProductQueryRequest, GetReservedProductQueryResponse>
{
    private readonly IMongoSellerProductRepository _sellerProductRepository = sellerProductRepository;
    private readonly IMongoColorRepository _colorRepository = colorRepository;

    public async Task<GetReservedProductQueryResponse> Handle(GetReservedProductQueryRequest request,
        CancellationToken cancellationToken)
    {
        var reserve = await _sellerProductRepository.FindReserveAsync(request.ProductId, request.ColorId,request.SellerId)
                      ?? throw new NotFoundException("محصولی با این مشخصات توسط شما رزور نشده");
        var color = await _colorRepository.FindByIdAsync(request.ColorId)
                    ?? throw new CustomInternalServerException(["Color Not Found"]);
        var model = new ShowReservedProductDto
        {
            ProductId = request.ProductId,
            ColorId = request.ColorId,
            Count = reserve.Count,
            BasePrice = reserve.BasePrice,
            Title = reserve.Product.Title,
            Image = reserve.Product.Images.First(),
            ColorCode = color.ColorCode,
            DiscountPercentage = reserve.DiscountPercentage,
            EndOfDiscount = reserve.EndOfDiscount
        };
        return new GetReservedProductQueryResponse(model);
    }
}