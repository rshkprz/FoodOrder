using FoodOrder.Api.Features.Auth.Constants;
using FoodOrder.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace FoodOrder.Api.Features.Auth.Register;

public static class RegisterEndpoint
{
    public static void MapRegister(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register", async (
            RegisterRequest request,
            UserManager<ApplicationUser> userManager) =>
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return Results.BadRequest(result.Errors);
            
            await userManager.AddToRoleAsync(user, Roles.Customer);

            return Results.Ok();
        });
    }
}
