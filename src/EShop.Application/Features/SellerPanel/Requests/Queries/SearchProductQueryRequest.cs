namespace EShop.Application.Features.SellerPanel.Requests.Queries;

public record SearchProductQueryRequest: IRequest<SearchProductQueryResonse>
{
    [DisplayName("عنوان")]
    public string Title { get; set; } = string.Empty;
}
public record SearchProductQueryResonse(List<ShowAllProductDto> Products);
