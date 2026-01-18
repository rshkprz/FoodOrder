using System;
using FoodOrder.Api.Features.Auth.Jwt;
using FoodOrder.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FoodOrder.Api.Features.Auth.Login;

public static class LoginEndpoint
{
    public static void MapLogin(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (
            LoginRequest request,
            UserManager<ApplicationUser> userManager,
            IOptions<JwtOptions> jwtOptions) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Results.Unauthorized();

            if (!await userManager.CheckPasswordAsync(user, request.Password))
                return Results.Unauthorized();

            var roles = await userManager.GetRolesAsync(user);

            Console.WriteLine($"User found → Id: '{user.Id}', Email: '{user.Email}', Username: '{user.UserName}'");
            Console.WriteLine(string.Join(", ", roles));

            if (string.IsNullOrEmpty(user.Id))
            {
                return Results.Problem("User ID is null - cannot generate token");
            }

            var token = JwtTokenGenerator.Generate(user, roles, jwtOptions.Value);

            return Results.Ok(new LoginResponse(token));
        });
    }
}
