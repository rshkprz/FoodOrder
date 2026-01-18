using System;
using FoodOrder.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Api.Features.Categories.GetCategories;

public class GetCategoriesEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/categories", async (AppDbContext db) =>
        {
            var categories = await db.Categories
            .Where(c => c.IsActive)
            .OrderBy(c=>c.Name)
            .Select(c=>new CategoryListItem(
                c.Id,
                c.Name
            ))
            .ToListAsync();

            return Results.Ok(categories);
        })
        .WithTags("Categories");
    }
}
