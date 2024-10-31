using EShop.Application.Common.Exceptions;
using EShop.Application.Features.Authorize.Requests.Commands;

namespace EShop.Application.Features.Authorize.Handlers.Commands;

public class VerifyPhoneNumberCommandHandler(IApplicationUserManager userManager,
    [FromKeyedServices("email")] IEmailSenderService emailSender,
    ILogger<RegisterCommandHandler> logger) : IRequestHandler<VerifyPhoneNumberCommandRequest, VerifyPhoneNumberCommandResponse>
{
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly IEmailSenderService _emailSender = emailSender;
    private readonly ILogger<RegisterCommandHandler> _logger = logger;

    public async Task<VerifyPhoneNumberCommandResponse> Handle(VerifyPhoneNumberCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByPhoneNumberAsync(request.PhoneNumber) ??
            throw new NotFoundException("کاربر");
        if (user.PhoneNumberConfirmed)
        {
            throw new CustomBadRequestException([Errors.PhoneNumberAlreadyVerified]);
        }

        var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, request.Code,user.PhoneNumber!);
        if (!result)
        {
            throw new CustomBadRequestException([Errors.InvalidCode]);
        }
        user.PhoneNumberConfirmed = true;
        user.IsActive = true;
        await _userManager.UpdateUserAsync(user);
        _logger.LogInformation($"user with phone number: {user.PhoneNumber} verified");

        return new();
    }
}
