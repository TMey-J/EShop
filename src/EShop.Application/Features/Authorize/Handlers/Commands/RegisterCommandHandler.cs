using EShop.Application.Common.Helpers;
using EShop.Application.Contracts.Identity;
using EShop.Application.Features.Authorize.Requests.Commands;
using EShop.Domain.Entities.Identity;
using MediatR;
using Microsoft.Extensions.Options;
using Restaurant.Application.Models;

namespace EShop.Application.Features.Authorize.Handlers.Commands;

public class RegisterCommandHandler(IApplicationUserManager userManager,IOptionsMonitor<SiteSettings> siteSettings) : IRequestHandler<RegisterCommandRequest, RegisterCommandRespinse>
{
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly SiteSettings _siteSettings = siteSettings.CurrentValue;

    public async Task<RegisterCommandRespinse> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
    {
        var isEmail = request.EmailOrPhoneNumber.IsEmail();
        var user = isEmail ?
            await _userManager.FindByEmailAsync(request.EmailOrPhoneNumber) :
            await _userManager.FindByNameAsync(request.EmailOrPhoneNumber);
        if (user is not null)
        {
            //TODO throw custom exception
            throw new Exception();
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
            //TODO Log
            //TODO throw exception
            throw new Exception();

        }
        if (isEmail)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //TODO send email
        }
        else
        {
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user,user.PhoneNumber!);
            //TODO send sms
        }
        return new RegisterCommandRespinse(request.EmailOrPhoneNumber, _siteSettings.WaitForSendCodeSeconds, user.SendCodeLastTime);

    }
}
