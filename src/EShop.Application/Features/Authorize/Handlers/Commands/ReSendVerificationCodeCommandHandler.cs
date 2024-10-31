using EShop.Application.Common.Exceptions;
using EShop.Application.Features.Authorize.Requests.Commands;
using Microsoft.AspNetCore.WebUtilities;

namespace EShop.Application.Features.Authorize.Handlers.Commands;

public class ReSendVerificationCodeCommandHandler(IApplicationUserManager userManager,
    [FromKeyedServices("sms")] ISmsSenderService smsSender,
    [FromKeyedServices("email")] IEmailSenderService emailSender,
    IOptionsSnapshot<SiteSettings> siteSettings,
    ILogger<RegisterCommandHandler> logger) : IRequestHandler<ReSendVerificationCideCommandRequest, ReSendVerificationCideCommandRespose>
{
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly ISmsSenderService _smsSender = smsSender;
    private readonly IEmailSenderService _emailSender = emailSender;
    private readonly SiteSettings _siteSettings = siteSettings.Value;
    private readonly ILogger<RegisterCommandHandler> _logger = logger;

    public async Task<ReSendVerificationCideCommandRespose> Handle(ReSendVerificationCideCommandRequest request, CancellationToken cancellationToken)
    {
        var isEmail = request.EmailOrPhoneNumber.IsEmail();
        var user = isEmail ?
            await _userManager.FindByEmailAsync(request.EmailOrPhoneNumber) :
            await _userManager.FindByPhoneNumberAsync(request.EmailOrPhoneNumber)
            ?? throw new NotFoundException("کاربر");

        if (user.PhoneNumberConfirmed)
        {
            throw new CustomBadRequestException([Errors.PhoneNumberAlreadyVerified]);
        }
        var dateTImeNow = DateTime.Now;
        var sendCodeLastTime = user.SendCodeLastTime;
        if (dateTImeNow < sendCodeLastTime.AddSeconds(_siteSettings.WaitForSendCodeSeconds))
        {
            throw new CustomBadRequestException([Errors.InvalidTimeToSendCode]);
        }
        if (isEmail)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token},
                {"email", user.Email}
            };
            var url = QueryHelpers.AddQueryString(string.Empty, param);// the url should be filled based on the address of the front-end page
            var emailBody = EmailTemplates.VerifyUserCodeEmail(url, user.Email!);
            await _emailSender.SendEmailAsync(user.Email!, Subjects.VeryfyCodeMailSubject, emailBody);
        }
        else
        {
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber!);
            await _smsSender.SendOTP(user.PhoneNumber!, _siteSettings.SmsSettings.LoginCodeTemplateName, code);
        }
        
        user.SendCodeLastTime = dateTImeNow;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new CustomInternalServerException(result.GetErrors(), Errors.InternalServer);
        }

        return new(user.PhoneNumber!, _siteSettings.WaitForSendCodeSeconds, dateTImeNow);
    }
}
