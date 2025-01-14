using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.SellerPanel.Requests.Queries;

namespace EShop.Application.Features.SellerPanel.Handlers.Queries;

public class SearchProductQueryHandler(
    IMongoProductRepository productRepository)
    : IRequestHandler<SearchProductQueryRequest, SearchProductQueryResponse>
{
    private readonly IMongoProductRepository _productRepository = productRepository;

    public async Task<SearchProductQueryResponse> Handle(SearchProductQueryRequest request,
        CancellationToken cancellationToken)
    {
        var products = await _productRepository.SearchProductByTitleAsync(request.Title,cancellationToken);
        var model = products.Select(x => new ShowAllProductDto
        {
            Id = x.Id,
            Title = x.Title,
            EnglishTitle = x.EnglishTitle,
            CategoryTitle = x.CategoryTitle,
            Image = x.Images.First()
        }).ToList();
        return new SearchProductQueryResponse(model);
    }
}