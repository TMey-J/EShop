using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Product.Requests.Queries;

namespace EShop.Application.Features.AdminPanel.Product.Handlers.Queries;

public class GetProductQueryHandler(IMongoProductRepository product) :
    IRequestHandler<GetProductQueryRequest, GetProductQueryResponse>
{
    private readonly IMongoProductRepository _product = product;

    public async Task<GetProductQueryResponse> Handle(GetProductQueryRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _product.FindByIdAsync(request.Id)
                      ?? throw new NotFoundException(NameToReplaceInException.Product);
        var totalCount = await _product.CountProductByIdAsync(product.Id);
        uint priceWithDiscount = 0;
        if (product.DiscountPercentage > 0)
        {
            priceWithDiscount=MathHelper.CalculatePriceWithDiscount(product.BasePrice, product.DiscountPercentage);
        }

        var responseModel = new ShowProductDto
        {
            Id = product.Id,
            Title = product.Title,
            EnglishTitle = product.EnglishTitle,
            BasePrice = product.BasePrice,
            Count = totalCount,
            DiscountPercentage = product.DiscountPercentage,
            PriceWithDiscount = priceWithDiscount,
            EndOfDiscount = product.EndOfDiscount,
            CategoryId = product.CategoryId,
            Images = product.Images,
            Tags = product.Tags,
            ColorsCode = product.Colors.Select(x => x.Key).ToList()
        };
        return new GetProductQueryResponse(responseModel);
    }
}