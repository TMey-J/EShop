using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.User.Requests.Queries;

namespace EShop.Application.Features.AdminPanel.User.Handlers.Queries;

public class GetUserQueryHandler(IMongoUserRepository user, IOptionsSnapshot<SiteSettings> siteSettings) :
    IRequestHandler<GetUserQueryRequest, GetUserQueryResponse>
{
    private readonly IMongoUserRepository _user = user;
    private readonly string _defaultUserAvatar = siteSettings.Value.DefaultUserAvatar;

    public async Task<GetUserQueryResponse> Handle(GetUserQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _user.FindByIdAsync(request.Id) ??
                   throw new NotFoundException(NameToReplaceInException.User);

        var showUser = new ShowUserDto(
            user.Id,
            user.UserName,
            user.Email ?? user.PhoneNumber,
            user.IsActive,
            user.Avatar ?? _defaultUserAvatar
            );
        return new GetUserQueryResponse(showUser);
    }
}