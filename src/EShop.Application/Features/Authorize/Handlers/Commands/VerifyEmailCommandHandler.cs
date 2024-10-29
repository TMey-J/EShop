using EShop.Application.Common.Exceptions;
using EShop.Application.Features.Authorize.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace EShop.Application.Features.Authorize.Handlers.Commands;

public class VerifyEmailCommandHandler(IApplicationUserManager userManager,
    IOptionsMonitor<SiteSettings> siteSettings,
    [FromKeyedServices("gmail")]IEmailSenderService emailSender,
    ILogger<RegisterCommandHandler> logger,IHttpContextAccessor httpContext) : IRequestHandler<VerifyEmailCommandRequest, VerifyEmailCommandResponse>
{
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly IEmailSenderService _emailSender = emailSender;
    private readonly ILogger<RegisterCommandHandler> _logger = logger;
    private readonly IHttpContextAccessor _httpContext = httpContext;
    private readonly SiteSettings _siteSettings = siteSettings.CurrentValue;

    public async Task<VerifyEmailCommandResponse> Handle(VerifyEmailCommandRequest request, CancellationToken cancellationToken)
    {
        var user=await _userManager.FindByEmailAsync(request.Email)??
            throw new NotFoundException("کاربر");

        var result = await _userManager.ConfirmEmailAsync(user, request.Token) ??
            throw new CustomBadRequestException([Messages.Errors.InvalidToken]);

        return new();
    }
}
