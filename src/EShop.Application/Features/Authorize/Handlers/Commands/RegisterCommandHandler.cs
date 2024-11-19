using EShop.Application.Features.Authorize.Requests.Commands;
using Microsoft.AspNetCore.WebUtilities;

namespace EShop.Application.Features.Authorize.Handlers.Commands;

public class RegisterCommandHandler(IApplicationUserManager userManager,
    IOptionsMonitor<SiteSettings> siteSettings,
    IEmailSenderService emailSender,
    ILogger<RegisterCommandHandler> logger,
    ISmsSenderService smsSender) : IRequestHandler<RegisterCommandRequest, RegisterCommandResponse>
{
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly IEmailSenderService _emailSender = emailSender;
    private readonly ISmsSenderService _smsSender = smsSender;
    private readonly ILogger<RegisterCommandHandler> _logger = logger;
    private readonly SiteSettings _siteSettings = siteSettings.CurrentValue;

    public async Task<RegisterCommandResponse> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
    {
        (User? value,bool isEmail) user= await _userManager.FindByEmailOrPhoneNumberWithCheckIsEmailAsync(request.EmailOrPhoneNumber);
        if (user.value is not null)
        {
            throw new DuplicateException(NameToReplaceInException.User);
        }
        user.value=new()
        {
            UserName = request.UserName,
            Email = user.isEmail ? request.EmailOrPhoneNumber : null,
            PhoneNumber = user.isEmail ? null : request.EmailOrPhoneNumber,
            PasswordHash = request.Password.HashPassword(out var salt),
            PasswordSalt = salt
        };
        var result = await _userManager.CreateAsync(user.value);
        if (!result.Succeeded)
        {
            throw new CustomInternalServerException(result.GetErrors(),Errors.InternalServer);
        }
        if (user.value.Email is not null)
        {
            _logger.LogInformation($"user with email: {user.value.Email} registered");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user.value);
            var param = new Dictionary<string, string?>
            {
                {"token", token},
                {"email", user.value.Email}
            };
            var url = QueryHelpers.AddQueryString(string.Empty, param);// the url should be filled based on the address of the front-end page
            var emailBody = EmailTemplates.VerifyUserCodeEmail(url, user.value.Email);
            await _emailSender.SendEmailAsync(user.value.Email, Subjects.VeryfyCodeMailSubject, emailBody);
            return new RegisterCommandResponse(request.EmailOrPhoneNumber,null,null);

        }
        else
        {
            _logger.LogInformation($"user with phone number: {user.value.PhoneNumber} registered");
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user.value, user.value.PhoneNumber!);
            await _smsSender.SendOTP(user.value.PhoneNumber!, _siteSettings.SmsSettings.LoginCodeTemplateName, code);
            return new RegisterCommandResponse(request.EmailOrPhoneNumber, _siteSettings.WaitForSendCodeSeconds, user.value.SendCodeLastTime);

        }

    }
}
