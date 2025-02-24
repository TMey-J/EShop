﻿using EShop.Application.Constants;
using EShop.Application.Contracts.Identity;
using EShop.Application.Model;
using EShop.Domain.Entities.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Application.Contracts.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EShop.Infrastructure.Repositories.Identity;

public class JwtService(IOptionsMonitor<SiteSettings> options, IApplicationSignInManager signInManager) : IJwtService
{
    private readonly JwtConfigs _jwtConfigs = options.CurrentValue.JwtConfigs;
    private readonly IApplicationSignInManager _signInManager = signInManager;

    public async Task<string> GenerateAsync(User user)
    {
        var secretKey = Encoding.UTF8.GetBytes(_jwtConfigs.SecretKey);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

        var claims = await GetClaims(user);
        var tokenOptions = new JwtSecurityToken(
            _jwtConfigs.Issuer,
            _jwtConfigs.Audience,
            claims,
            DateTime.Now.AddMinutes(_jwtConfigs.NotBeforeMinutes),
            DateTime.Now.AddMinutes(_jwtConfigs.ExpirationMinutes),
            signingCredentials);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return token;
    }

    private async Task<IEnumerable<Claim>> GetClaims(User user)
    {
        var result = await _signInManager.ClaimsFactory.CreateAsync(user);
        var claims = new List<Claim>(result.Claims);
        if (user.PhoneNumber is not null)
        {
            claims.Add(new(ClaimsName.PhoneNumber, user.PhoneNumber));
        }
        return claims;
    }
}