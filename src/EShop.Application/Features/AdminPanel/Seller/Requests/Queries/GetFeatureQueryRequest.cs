namespace EShop.Application.Features.AdminPanel.Seller.Requests.Queries;

public record GetSellerQueryRequest: IRequest<GetSellerQueryResponse>
{
    [DisplayName("شناسه")]
    public long Id { get; init; }
}
public record GetSellerQueryResponse(ShowSellerDto Seller);