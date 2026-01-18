using System;
using FoodOrder.Api.Features.Auth.Constants;
using FoodOrder.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace FoodOrder.Api;

public static class Seed
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var config = services.GetRequiredService<IConfiguration>();

        await SeedRolesAsync(roleManager);
        await SeedDefaultAdminAsync(userManager, config);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = [Roles.Admin, Roles.Customer];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                Console.WriteLine("role seeded");
            }
        }

    }

    // private static async Task SeedDefaultAdminAsync(UserManager<ApplicationUser> userManager, IConfiguration config)
    // {
    //     var adminEmail = config["AdminCredentials:Email"];
    //     var adminPassword = config["AdminCredentials:Password"];

    //     if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
    //     {
    //         Console.WriteLine("Admin credentials not found in configuration!");
    //         return;
    //     }

    //     var admin = await userManager.FindByEmailAsync(adminEmail);

    //     if (admin != null)
    //     {
    //         Console.WriteLine($"Admin user {adminEmail} already exists.");
    //         return;
    //     }

    //     var adminUser = new ApplicationUser
    //     {
    //         UserName = adminEmail,
    //         Email = adminEmail,
    //         EmailConfirmed = true
    //     };

    //     var result = await userManager.CreateAsync(adminUser, adminPassword);

    //     if (result.Succeeded)
    //     {
    //         var roleResult = await userManager.AddToRoleAsync(adminUser, Roles.Admin);

    //         if (roleResult.Succeeded)
    //         {
    //             Console.WriteLine($"Admin user {adminEmail} created and assigned to role {Roles.Admin}");
    //         }
    //         else
    //         {
    //             Console.WriteLine("Failed to assign admin role:");
    //             foreach (var error in roleResult.Errors)
    //                 Console.WriteLine($"  • {error.Description}");
    //         }
    //     }
    //     else
    //     {
    //         Console.WriteLine($"Failed to create admin user {adminEmail}:");
    //         foreach (var error in result.Errors)
    //         {
    //             Console.WriteLine($"  • {error.Description}");
    //         }
    //     }
    // }

    private static async Task SeedDefaultAdminAsync(UserManager<ApplicationUser> userManager, IConfiguration config)
    {
        var adminEmail = config["AdminCredentials:Email"];
        var adminPassword = config["AdminCredentials:Password"];

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            Console.WriteLine("Admin credentials missing!");
            return;
        }

        // Check if already exists
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin != null)
        {
            Console.WriteLine($"Admin already exists (Id: {admin.Id})");
            return;
        }

        var newAdmin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
        };

        var createResult = await userManager.CreateAsync(newAdmin, adminPassword);

        if (!createResult.Succeeded)
        {
            Console.WriteLine("Create admin failed:");
            foreach (var error in createResult.Errors)
                Console.WriteLine("  • " + error.Description);
            return;
        }

        
        admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null || string.IsNullOrEmpty(admin.Id))
        {
            Console.WriteLine("CRITICAL: Admin created but Id is still null!");
            return;
        }

        var roleResult = await userManager.AddToRoleAsync(admin, Roles.Admin);

        if (roleResult.Succeeded)
        {
            Console.WriteLine($"Admin seeded OK → Id: {admin.Id}, Email: {adminEmail}");
        }
        else
        {
            Console.WriteLine("Role assignment failed:");
            foreach (var error in roleResult.Errors)
                Console.WriteLine("  • " + error.Description);
        }
    }
}
