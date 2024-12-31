using EShop.Application.Features.AdminPanel.User.Requests.Commands;
using EShop.Application.Features.Authorize.Handlers.Commands;
using EShop.Domain.Entities.Mongodb;

namespace EShop.Application.Features.AdminPanel.User.Handlers.Commands;

public class UpdateUserCommandHandler(
    IApplicationUserManager userManager,
    IApplicationRoleManager roleManager,
    IFileRepository fileRepository,
    IOptionsSnapshot<SiteSettings> siteSettings,
    ILogger<RegisterCommandHandler> logger,
    IRabbitmqPublisherService rabbitmqPublisher) : IRequestHandler<UpdateUserCommandRequest, UpdateUserCommandResponse>
{
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly IApplicationRoleManager _roleManager = roleManager;
    private readonly IRabbitmqPublisherService _rabbitmqPublisher = rabbitmqPublisher;
    private readonly IFileRepository _fileRepository = fileRepository;
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
        SaveFileBase64Model? saveFile = null;

        if (!string.IsNullOrWhiteSpace(request.Avatar))
        {
            saveFile = _fileRepository.ReadyToSaveFileAsync(request.Avatar,
                _siteSettings.FilesPath.SellerLogo, MaximumFilesSizeInMegaByte.SellerLogo,user.Avatar);
            user.Avatar = saveFile.fileNameWithExtention;
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
        
        await using var transaction = await _userManager.BeginTransactionAsync();

        try
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new CustomBadRequestException(result.GetErrors());
            }
            if (saveFile is not null)
            {
                await _fileRepository.SaveFileAsync(saveFile);
            }
            var mongoUser = new MongoUser{
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PasswordSalt = user.PasswordSalt,
                Avatar = user.Avatar,
                IsActive = true,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Id = user.Id,
                NormalizedUserName = user.NormalizedUserName,
                NormalizedEmail = user.NormalizedEmail,
                ConcurrencyStamp = user.ConcurrencyStamp,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,
                SecurityStamp = user.SecurityStamp,
                AccessFailedCount = user.AccessFailedCount,
                TwoFactorEnabled = user.TwoFactorEnabled,
                SendCodeLastTime = user.SendCodeLastTime
            };
            if (user.PasswordHash is not null)
            {
                mongoUser.PasswordHash = user.PasswordHash;
            }
            await _rabbitmqPublisher.PublishMessageAsync<Domain.Entities.Identity.User>(
                new(ActionTypes.Create,user),
                RabbitmqConstants.QueueNames.User,
                RabbitmqConstants.RoutingKeys.User);
        
            _logger.LogInformation(
                $"user with Id {user.Id} has been updated by admin");
            return new UpdateUserCommandResponse();
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            
            throw;
        }
    }
}