using EShop.Application.Features.Authorize.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace EShop.Application.Features.Authorize.Handlers.Commands;

public class RegisterCommandHandler(IApplicationUserManager userManager,
    IOptionsMonitor<SiteSettings> siteSettings,
    [FromKeyedServices("email")] IEmailSenderService emailSender,
    ILogger<RegisterCommandHandler> logger,
    [FromKeyedServices("sms")] ISmsSenderService smsSender) : IRequestHandler<RegisterCommandRequest, RegisterCommandResponse>
{
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly IEmailSenderService _emailSender = emailSender;
    private readonly ISmsSenderService _smsSender = smsSender;
    private readonly ILogger<RegisterCommandHandler> _logger = logger;
    private readonly SiteSettings _siteSettings = siteSettings.CurrentValue;

    public async Task<RegisterCommandResponse> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
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
            PasswordSalt = salt
        };
        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            throw new CustomInternalServerException(result.GetErrors(),Errors.InternalServer);
        }
        if (user.Email is not null)
        {
            _logger.LogInformation($"user with email: {user.Email} registered");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token},
                {"email", user.Email}
            };
            var url = QueryHelpers.AddQueryString(string.Empty, param);// the url should be filled based on the address of the front-end page
            var emailBody = EmailTemplates.VerifyUserCodeEmail(url, user.Email);
            await _emailSender.SendEmailAsync(user.Email, Subjects.VeryfyCodeMailSubject, emailBody);
            return new RegisterCommandResponse(request.EmailOrPhoneNumber,null,null);

        }
        else
        {
            _logger.LogInformation($"user with phone number: {user.PhoneNumber} registered");
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber!);
            await _smsSender.SendOTP(user.PhoneNumber!, _siteSettings.SmsSettings.LoginCodeTemplateName, code);
            return new RegisterCommandResponse(request.EmailOrPhoneNumber, _siteSettings.WaitForSendCodeSeconds, user.SendCodeLastTime);

        }

    }
}
