using EShop.Application.Common.Exceptions;
using EShop.Application.Features.Authorize.Requests.Commands;
using Microsoft.AspNetCore.WebUtilities;

namespace EShop.Application.Features.Authorize.Handlers.Commands;

public class ReSendVerificationCodeCommandHandler(IApplicationUserManager userManager,
    ISmsSenderService smsSender,
    IEmailSenderService emailSender,
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
        (User? value, bool isEmail) user = await _userManager.FindByEmailOrPhoneNumberAsync(request.EmailOrPhoneNumber);

        if (user.value is null)
        {
            throw new NotFoundException("کاربر");
        }

        if (user.value.PhoneNumberConfirmed)
        {
            throw new CustomBadRequestException([Errors.PhoneNumberAlreadyVerified]);
        }
        var dateTImeNow = DateTime.Now;
        var sendCodeLastTime = user.value.SendCodeLastTime;
        if (dateTImeNow < sendCodeLastTime.AddSeconds(_siteSettings.WaitForSendCodeSeconds))
        {
            throw new CustomBadRequestException([Errors.InvalidTimeToSendCode]);
        }
        if (user.isEmail)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user.value);
            var param = new Dictionary<string, string?>
            {
                {"token", token},
                {"email", user.value.Email}
            };
            var url = QueryHelpers.AddQueryString(string.Empty, param);// the url should be filled based on the address of the front-end page
            var emailBody = EmailTemplates.VerifyUserCodeEmail(url, user.value.Email!);
            await _emailSender.SendEmailAsync(user.value.Email!, Subjects.VeryfyCodeMailSubject, emailBody);
        }
        else
        {
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user.value, user.value.PhoneNumber!);
            await _smsSender.SendOTP(user.value.PhoneNumber!, _siteSettings.SmsSettings.LoginCodeTemplateName, code);
        }
        
        user.value.SendCodeLastTime = dateTImeNow;
        var result = await _userManager.UpdateAsync(user.value);
        if (!result.Succeeded)
        {
            throw new CustomInternalServerException(result.GetErrors(), Errors.InternalServer);
        }

        return new(user.value.PhoneNumber!, _siteSettings.WaitForSendCodeSeconds, dateTImeNow);
    }
}
