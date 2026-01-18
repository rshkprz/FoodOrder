using System.Text;
using FoodOrder.Api;
using FoodOrder.Api.Data;
using FoodOrder.Api.Features.Auth;
using FoodOrder.Api.Features.Auth.Jwt;
using FoodOrder.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options=>options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true, 
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
            ValidAudience = builder.Configuration["JwtOptions:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:SecretKey"]!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (args.Contains("--seed"))
{
    Console.WriteLine("Seeding database...");
    try
    {
        await Seed.InitializeAsync(app.Services);  // ← make it async
        Console.WriteLine("Seeding completed successfully.");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine("Seeding failed!");
        Console.Error.WriteLine(ex);
        Environment.ExitCode = 1;
    }
    return;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();


app.Run();

