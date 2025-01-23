namespace EShop.Application.Features.SellerPanel.Requests.Queries;

public record ShowProductForSellerPanelQueryRequest: IRequest<ShowProductQueryResponse>
{
    [DisplayName("شناسه")]
    public long Id { get; set; }
}
public record ShowProductQueryResponse(ShowProductForSellerPanelDto Product);
