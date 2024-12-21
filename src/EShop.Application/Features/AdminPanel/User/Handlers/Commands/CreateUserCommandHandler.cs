using EShop.Application.Features.AdminPanel.User.Requests.Commands;
using EShop.Application.Features.Authorize.Handlers.Commands;

namespace EShop.Application.Features.AdminPanel.User.Handlers.Commands;

public class CreateUserCommandHandler(
    IApplicationUserManager userManager,
    IApplicationRoleManager roleManager,
    IFileRepository fileRepository,
    IOptionsSnapshot<SiteSettings> siteSettings,
    ILogger<RegisterCommandHandler> logger,
    IRabbitmqPublisherService rabbitmqPublisher) : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
{
    private readonly FilesPath _filesPath = siteSettings.Value.FilesPath;
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly IApplicationRoleManager _roleManager = roleManager;
    private readonly IFileRepository _fileRepository = fileRepository;
    private readonly IRabbitmqPublisherService _rabbitmqPublisher = rabbitmqPublisher;
    private readonly ILogger<RegisterCommandHandler> _logger = logger;

    public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request,
        CancellationToken cancellationToken)
    {
        (Domain.Entities.Identity.User? user, bool isEmail) userFound =
            await _userManager.FindByEmailOrPhoneNumberWithCheckIsEmailAsync(request.EmailOrPhoneNumber);
        if (userFound.user is not null)
        {
            throw new DuplicateException("کاربر");
        }

        var user = new Domain.Entities.Identity.User()
        {
            UserName = request.UserName,
            Email = userFound.isEmail ? request.EmailOrPhoneNumber : null,
            PhoneNumber = userFound.isEmail ? null : request.EmailOrPhoneNumber,
            PasswordHash = request.Password.HashPassword(out var salt),
            PasswordSalt = salt,
            IsActive = true,
        };
        SaveFileBase64Model? saveFile = null;

        if (!string.IsNullOrWhiteSpace(request.Avatar))
        {
            saveFile = _fileRepository.ReadyToSaveFileAsync(request.Avatar,
                _filesPath.SellerLogo, MaximumFilesSizeInMegaByte.SellerLogo);
            user.Avatar = saveFile.fileNameWithExtention;
        }

        if (userFound.isEmail)
        {
            user.EmailConfirmed = true;
        }
        else
        {
            user.PhoneNumberConfirmed = true;
        }

        
        await using var transaction = await _userManager.BeginTransactionAsync();
        try
        {
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                throw new CustomInternalServerException(result.GetErrors());
            }
            if (saveFile is not null)
            {
                await _fileRepository.SaveFileAsync(saveFile);
            }

            #region role logic

            request.Roles = request.Roles.Select(x => x.ToUpper()).ToList();
            var notExistsRolesName = await _roleManager.NotExistsRolesNameAsync(request.Roles);
            if (notExistsRolesName.Count > 0)
            {
                throw new CustomBadRequestException(Errors.NotExistsRolesErrors(notExistsRolesName));
            }

            var addToRulesResult = await _userManager.AddToRolesAsync(user, request.Roles);
            if (!addToRulesResult.Succeeded)
            {
                throw new CustomBadRequestException(addToRulesResult.GetErrors());
            }


            #endregion
            
            await _userManager.UpdateAsync(user);

            user.UserRoles = null;
            user.UserClaims = null;
            user.UserTokens = null;
            user.UserLogins = null;
            await _rabbitmqPublisher.PublishMessageAsync<Domain.Entities.Identity.User>(
                new(ActionTypes.Create, user),
                RabbitmqConstants.QueueNames.User,
                RabbitmqConstants.RoutingKeys.User);
            
            await transaction.CommitAsync(cancellationToken);
            _logger.LogInformation(
                $"user with phone Number/email {user.Email ?? user.PhoneNumber} has been created by admin");
            return new CreateUserCommandResponse();
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            
            if (saveFile is not null)
            {
                _fileRepository.DeleteFile(saveFile.fileNameWithExtention, _filesPath.UserAvatar);
            }
            throw;
        }
    }
}