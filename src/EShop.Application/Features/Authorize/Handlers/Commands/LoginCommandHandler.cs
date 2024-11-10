using EShop.Application.Features.Authorize.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Restaurant.Application.Contracts.Identity;
using System;

namespace EShop.Application.Features.Authorize.Handlers.Commands;

public class LoginCommandHandler(IApplicationUserManager userManager,
    IJwtService jwt,
    ILogger<RegisterCommandHandler> logger) : IRequestHandler<LoginCommandRequest, LoginCommandResponde>
{
    private readonly IApplicationUserManager _userManager = userManager;
    private readonly IJwtService _jwt = jwt;
    private readonly ILogger<RegisterCommandHandler> _logger = logger;

    public async Task<LoginCommandResponde> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
    {
        (User? value, bool isEmail) user = await _userManager.FindByEmailOrPhoneNumberAsync(request.EmailOrPhoneNumber);

        if (user.value is null)
        {
            throw new NotFoundException("کاربر");
        }
        if (!user.value.IsActive)
        {
            throw new CustomBadRequestException([Errors.UserNotActive]);
        }
        var verifyPassword = PasswordHelper.VerifyPassword(request.Password, user.value.PasswordHash!, user.value.PasswordSalt);
        if (!verifyPassword)
        {
            throw new NotFoundException("کاربر");
        }

        var token = await _jwt.GenerateAsync(user.value);

        if (user.isEmail)
        {
            _logger.LogInformation($"user with email {user.value.Email} loged in");
        }
        else
        {
            _logger.LogInformation($"user with phone number {user.value.PhoneNumber} loged in");

        }

        return new(token);
    }
}
