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
        (Domain.Entities.Identity.User? value,bool isEmail) user= await _userManager.FindByEmailOrPhoneNumberAsync(request.EmailOrPhoneNumber);
        if (user.value is not null)
        {
            throw new DuplicateException("کاربر");
        }
        user.value=new()
        {
            UserName = request.UserName,
            Email = user.isEmail ? request.EmailOrPhoneNumber : null,
            PhoneNumber = user.isEmail ? null : request.EmailOrPhoneNumber,
            PasswordHash = request.Password.HashPassword(out var salt),
            PasswordSalt = salt,
            IsActive = true,
            Avatar = !string.IsNullOrWhiteSpace(request.Avatar)?
                await request.Avatar.UploadFileAsync(_filesPath.UserAvatar):
                null
        };
        if (user.isEmail)
        {
            user.value.EmailConfirmed = true;
        }
        else
        {
            user.value.PhoneNumberConfirmed = true;
        }
        #region role logic

        request.Roles = request.Roles.Select(x => x.ToUpper()).ToList();
        var notExistsRolesName = await _roleManager.NotExistsRolesNameAsync(request.Roles);
        if (notExistsRolesName.Count > 0)
        {
            throw new CustomBadRequestException(Errors.NotExistsRolesErrors(notExistsRolesName));
        }             

        var addToRulesResult = await _userManager.AddToRolesAsync(user.value, request.Roles);
        if (!addToRulesResult.Succeeded)
        {
            throw new CustomBadRequestException(addToRulesResult.GetErrors());
        }

        #endregion
        var result = await _userManager.CreateAsync(user.value);
        if (!result.Succeeded)
        {
            throw new CustomInternalServerException(result.GetErrors());
        }
        _logger.LogInformation($"user with phone Number/email {user.value.Email??user.value.PhoneNumber} has been created by admin");
        return new CreateUserCommandResponse();
    }
}
