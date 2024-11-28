namespace EShop.Application.Features.AdminPanel.User.Requests.Queries;

public record GetAllUsersQueryRequest(SearchUserDto Search):IRequest<GetAllUsersQueryResponse>;
public record GetAllUsersQueryResponse(List<ShowUserDto> Users,SearchUserDto Search,int PageCount);