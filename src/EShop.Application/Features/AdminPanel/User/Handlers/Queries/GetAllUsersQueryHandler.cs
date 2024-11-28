using EShop.Application.Features.AdminPanel.User.Requests.Queries;

namespace EShop.Application.Features.AdminPanel.User.Handlers.Queries;

public class GetAllUsersQueryHandler(IApplicationUserManager userManager):
    IRequestHandler<GetAllUsersQueryRequest,GetAllUsersQueryResponse>
{
    private readonly IApplicationUserManager _userManager = userManager;

    public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
    {
        var users = await _userManager.GetAllWithFiltersAsync(request.Search);
        
        return users;
    }
}