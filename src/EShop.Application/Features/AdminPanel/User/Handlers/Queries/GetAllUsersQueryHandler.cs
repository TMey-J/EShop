using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.User.Requests.Queries;

namespace EShop.Application.Features.AdminPanel.User.Handlers.Queries;

public class GetAllUsersQueryHandler(IMongoUserRepository user):
    IRequestHandler<GetAllUsersQueryRequest,GetAllUsersQueryResponse>
{
    private readonly IMongoUserRepository _user = user;

    public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
    {
        var users = await _user.GetAllWithFiltersAsync(request.Search);
        
        return users;
    }
}