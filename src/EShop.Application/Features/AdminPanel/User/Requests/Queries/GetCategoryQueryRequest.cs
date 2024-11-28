namespace EShop.Application.Features.AdminPanel.User.Requests.Queries;

public record GetUserQueryRequest:IRequest<GetUserQueryResponse>
{
    [DisplayName("شناسه")]
    public long Id { get; set; }
}
public record GetUserQueryResponse(ShowUserDto User);