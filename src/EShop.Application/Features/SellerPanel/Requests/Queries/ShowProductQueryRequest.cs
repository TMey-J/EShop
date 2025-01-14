namespace EShop.Application.Features.SellerPanel.Requests.Queries;

public record ShowProductQueryRequest: IRequest<ShowProductQueryResponse>
{
    [DisplayName("شناسه")]
    public long Id { get; set; }
}
public record ShowProductQueryResponse(ShowProductForSellerPanelDto Product);
