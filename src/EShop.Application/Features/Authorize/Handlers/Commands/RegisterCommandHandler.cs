using EShop.Application.Features.Authorize.Requests.Commands;

namespace EShop.Application.Features.Authorize.Handlers.Commands;

public class RegisterCommandHandler(IApplicationUserManager userManager,
    IOptionsMonitor<SiteSettings> siteSettings,
    [FromKeyedServices("gmail")]IEmailSenderService emailSender,
    ILogger<RegisterCommandHandler> logger) : IRequestHandler<RegisterCommandRequest, RegisterCommandRespinse>
{
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly IEmailSenderService _emailSender = emailSender;
    private readonly ILogger<RegisterCommandHandler> _logger = logger;
    private readonly SiteSettings _siteSettings = siteSettings.CurrentValue;

    public async Task<RegisterCommandRespinse> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
    {
        var isEmail = request.EmailOrPhoneNumber.IsEmail();
        var user = isEmail ?
            await _userManager.FindByEmailAsync(request.EmailOrPhoneNumber) :
            await _userManager.FindByPhoneNumberAsync(request.EmailOrPhoneNumber);
        if (user is not null)
        {
            throw new DuplicateException("ایمیل / شماره تلفن");
        }
        user = new User
        {
            UserName = request.UserName,
            Email = isEmail ? request.EmailOrPhoneNumber : null,
            PhoneNumber = isEmail ? null : request.EmailOrPhoneNumber,
            PasswordHash = request.Password.HashPassword(out var salt),
            PasswordSalt = salt,
            SendCodeLastTime=DateTime.UtcNow,
        };
        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            throw new CustomInternalServerException("مشکلی در ثبت نام کاربر به وجود آمده");
        }
        if (user.Email is not null)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var emailBody=EmailTemplates.VerifyUserCodeEmail(code,user.Email);
            await _emailSender.SendEmailAsync(user.Email, Messages.Subjects.VeryfyCodeMailSubject, emailBody);
        }
        else
        {
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user,user.PhoneNumber!);
            //TODO send sms
        }
        return new RegisterCommandRespinse(request.EmailOrPhoneNumber, _siteSettings.WaitForSendCodeSeconds, user.SendCodeLastTime);

    }
}
