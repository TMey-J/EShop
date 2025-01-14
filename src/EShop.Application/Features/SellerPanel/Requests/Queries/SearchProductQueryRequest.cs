namespace EShop.Application.Features.SellerPanel.Requests.Queries;

public record SearchProductQueryRequest: IRequest<SearchProductQueryResponse>
{
    [DisplayName("عنوان")]
    public string Title { get; set; } = string.Empty;
}
public record SearchProductQueryResponse(List<ShowAllProductDto> Products);
