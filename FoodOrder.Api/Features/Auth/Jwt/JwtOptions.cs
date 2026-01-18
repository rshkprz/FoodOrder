using System;

namespace FoodOrder.Api.Features.Auth.Jwt;

public class JwtOptions
{
    public string SecretKey {get; set;} = null!;
    public string Issuer {get; set;} = null!;
    public string Audience {get; set;} = null!;   
}
