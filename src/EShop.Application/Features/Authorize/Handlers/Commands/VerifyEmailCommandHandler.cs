using EShop.Application.Common.Exceptions;
using EShop.Application.Features.Authorize.Requests.Commands;

namespace EShop.Application.Features.Authorize.Handlers.Commands;

public class VerifyEmailCommandHandler(IApplicationUserManager userManager,
     IEmailSenderService emailSender,
    ILogger<RegisterCommandHandler> logger) : IRequestHandler<VerifyEmailCommandRequest, VerifyEmailCommandResponse>
{
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly IEmailSenderService _emailSender = emailSender;
    private readonly ILogger<RegisterCommandHandler> _logger = logger;

    public async Task<VerifyEmailCommandResponse> Handle(VerifyEmailCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email) ??
            throw new NotFoundException("کاربر");
        if (user.EmailConfirmed)
        {
            throw new CustomBadRequestException([Messages.Errors.EmailAlreadyVerified]);
        }

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
        {
            throw new CustomBadRequestException([Messages.Errors.InvalidToken]);
        }
        user.IsActive = true;
        await _userManager.UpdateUserAsync(user);
        _logger.LogInformation($"user with email: {user.Email} verified");

        return new();
    }
}
