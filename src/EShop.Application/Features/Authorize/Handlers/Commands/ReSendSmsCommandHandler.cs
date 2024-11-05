using EShop.Application.Common.Exceptions;
using EShop.Application.Features.Authorize.Requests.Commands;

namespace EShop.Application.Features.Authorize.Handlers.Commands;

public class ReSendSmsCommandHandler(IApplicationUserManager userManager,
    ISmsSenderService smsSender, IOptionsSnapshot<SiteSettings> siteSettings,
    ILogger<RegisterCommandHandler> logger) : IRequestHandler<ReSendSmsCommandRequest, ReSendSmsCommandResponse>
{
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly ISmsSenderService _smsSender = smsSender;
    private readonly SiteSettings _siteSettings = siteSettings.Value;
    private readonly ILogger<RegisterCommandHandler> _logger = logger;

    public async Task<ReSendSmsCommandResponse> Handle(ReSendSmsCommandRequest request, CancellationToken cancellationToken)
    {
        var dateTImeNow=DateTime.Now;
        var user = await _userManager.FindByPhoneNumberAsync(request.PhoneNumber) ??
            throw new NotFoundException("کاربر");
        if (user.PhoneNumberConfirmed)
        {
            throw new CustomBadRequestException([Messages.Errors.PhoneNumberAlreadyVerified]);
        }

        var sendCodeLastTime = user.SendCodeLastTime;
        if (dateTImeNow < sendCodeLastTime.AddSeconds(_siteSettings.WaitForSendCodeSeconds))
        {
            throw new CustomBadRequestException([Messages.Errors.InvalidTimeToSendCode]);
        }
        var code=await _userManager.GenerateChangePhoneNumberTokenAsync(user,user.PhoneNumber!);
        await _smsSender.SendOTP(user.PhoneNumber!, _siteSettings.SmsSettings.LoginCodeTemplateName, code);
        user.SendCodeLastTime = dateTImeNow;
        var result= await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new CustomInternalServerException(result.GetErrors(),Messages.Errors.InternalServer);
        }
        return new(user.PhoneNumber!,_siteSettings.WaitForSendCodeSeconds, dateTImeNow);
    }
}
