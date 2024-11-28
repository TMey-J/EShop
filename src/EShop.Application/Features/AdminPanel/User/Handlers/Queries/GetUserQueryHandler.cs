using EShop.Application.Features.AdminPanel.User.Requests.Queries;

namespace EShop.Application.Features.AdminPanel.User.Handlers.Queries;

public class GetUserQueryHandler(IApplicationUserManager userManager, IOptionsSnapshot<SiteSettings> siteSettings) :
    IRequestHandler<GetUserQueryRequest, GetUserQueryResponse>
{
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly string _defaultUserAvatar = siteSettings.Value.DefaultUserAvatar;

    public async Task<GetUserQueryResponse> Handle(GetUserQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString()) ??
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