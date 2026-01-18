using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FoodOrder.Api.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace FoodOrder.Api.Features.Auth.Jwt;

public static class JwtTokenGenerator
{
    public static string Generate(ApplicationUser user, IList<string> roles, JwtOptions options)
    {
        if (string.IsNullOrEmpty(user.Id))
        {
            throw new InvalidOperationException("Cannot generate JWT: User Id is null or empty");
        }

        if (string.IsNullOrEmpty(user.Email))
        {
            throw new InvalidOperationException("Cannot generate JWT: User Email is null or empty");
        }
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!)
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
