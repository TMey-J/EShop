using EShop.Application.Features.AdminPanel.User.Requests.Commands;
using EShop.Application.Features.Authorize.Handlers.Commands;

namespace EShop.Application.Features.AdminPanel.User.Handlers.Commands;

public class UpdateUserCommandHandler(
    IApplicationUserManager userManager,
    IApplicationRoleManager roleManager,
    IFileRepository fileServices,
    IOptionsSnapshot<SiteSettings> siteSettings,
    ILogger<RegisterCommandHandler> logger) : IRequestHandler<UpdateUserCommandRequest, UpdateUserCommandResponse>
{
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly IApplicationRoleManager _roleManager = roleManager;
    private readonly IFileRepository _fileServices = fileServices;
    private readonly SiteSettings _siteSettings = siteSettings.Value;
    private readonly ILogger<RegisterCommandHandler> _logger = logger;

    public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommandRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
        {
            throw new NotFoundException(NameToReplaceInException.User);
        }

        user.UserName = request.UserName;
        user.IsActive = request.IsActive;

        user.Email = user.PhoneNumber = null;
        user.EmailConfirmed = user.PhoneNumberConfirmed = false;
        if (request.EmailOrPhoneNumber.IsEmail())
        {
            user.Email = request.EmailOrPhoneNumber;
            user.EmailConfirmed = true;
        }
        else
        {
            user.PhoneNumber = request.EmailOrPhoneNumber;
            user.PhoneNumberConfirmed = true;
        }

        if (request.Password is not null)
        {
            user.PasswordHash = request.Password.HashPassword(out var salt);
            user.PasswordSalt = salt;
        }

        #region role logic

        request.Roles = request.Roles.Select(x => x.ToUpper()).ToList();
        var notExistsRolesName = await _roleManager.NotExistsRolesNameAsync(request.Roles);
        if (notExistsRolesName.Count > 0)
        {
            throw new CustomBadRequestException(Errors.NotExistsRolesErrors(notExistsRolesName));
        }             
        
        var userRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, userRoles);

        var addToRulesResult = await _userManager.AddToRolesAsync(user, request.Roles);
        if (!addToRulesResult.Succeeded)
        {
            throw new CustomBadRequestException(addToRulesResult.GetErrors());
        }

        #endregion

        if (!string.IsNullOrWhiteSpace(request.NewAvatar))
        {
            user.Avatar = await _fileServices.UploadFileAsync
                (request.NewAvatar, _siteSettings.FilesPath.UserAvatar,
                    (int)FileHelpers.MaximumFilesSizeInMegaByte.UserAvatar,
                    user.Avatar);
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new CustomBadRequestException(result.GetErrors());
        }

        _logger.LogInformation(
            $"user with Id {user.Id} has been updated by admin");
        return new UpdateUserCommandResponse();
    }
}