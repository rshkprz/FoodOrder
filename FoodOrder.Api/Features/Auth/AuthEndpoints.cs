using System;
using FoodOrder.Api.Features.Auth.Login;
using FoodOrder.Api.Features.Auth.Register;

namespace FoodOrder.Api.Features.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRegister();
        app.MapLogin();
        return app;
    }
}
