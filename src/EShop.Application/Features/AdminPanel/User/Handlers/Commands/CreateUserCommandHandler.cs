using EShop.Application.Features.AdminPanel.User.Requests.Commands;
using EShop.Application.Features.Authorize.Handlers.Commands;

namespace EShop.Application.Features.AdminPanel.User.Handlers.Commands;

public class CreateUserCommandHandler(
    IApplicationUserManager userManager,IApplicationRoleManager roleManager,
    IOptionsSnapshot<SiteSettings> siteSettings,
    ILogger<RegisterCommandHandler> logger) : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
{
    private readonly FilesPath _filesPath = siteSettings.Value.FilesPath;
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly IApplicationRoleManager _roleManager = roleManager;
    private readonly ILogger<RegisterCommandHandler> _logger = logger;

    public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        (Domain.Entities.Identity.User? user, bool isEmail) userFound= await _userManager.FindByEmailOrPhoneNumberAsync(request.EmailOrPhoneNumber);
        if (userFound.user is not null)
        {
            throw new DuplicateException("کاربر");
        }
        userFound.user=new()
        {
            UserName = request.UserName,
            Email = userFound.isEmail ? request.EmailOrPhoneNumber : null,
            PhoneNumber = userFound.isEmail ? null : request.EmailOrPhoneNumber,
            PasswordHash = request.Password.HashPassword(out var salt),
            PasswordSalt = salt,
            IsActive = true,
            Avatar = !string.IsNullOrWhiteSpace(request.Avatar)?
                await request.Avatar.UploadFileAsync(_filesPath.UserAvatar):
                null
        };
        if (userFound.isEmail)
        {
            userFound.user.EmailConfirmed = true;
        }
        else
        {
            userFound.user.PhoneNumberConfirmed = true;
        }
        
        var result = await _userManager.CreateAsync(userFound.user);
        if (!result.Succeeded)
        {
            throw new CustomInternalServerException(result.GetErrors());
        }
        #region role logic

        request.Roles = request.Roles.Select(x => x.ToUpper()).ToList();
        var notExistsRolesName = await _roleManager.NotExistsRolesNameAsync(request.Roles);
        if (notExistsRolesName.Count > 0)
        {
            throw new CustomBadRequestException(Errors.NotExistsRolesErrors(notExistsRolesName));
        }             

        var addToRulesResult = await _userManager.AddToRolesAsync(userFound.user, request.Roles);
        if (!addToRulesResult.Succeeded)
        {
            throw new CustomBadRequestException(addToRulesResult.GetErrors());
        }

        await _userManager.UpdateAsync(userFound.user);
        #endregion
        _logger.LogInformation($"user with phone Number/email {userFound.user.Email??userFound.user.PhoneNumber} has been created by admin");
        return new CreateUserCommandResponse();
    }
}
