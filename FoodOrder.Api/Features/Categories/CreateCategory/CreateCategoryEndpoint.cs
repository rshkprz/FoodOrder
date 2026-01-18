using System;
using FoodOrder.Api.Data;
using FoodOrder.Api.Models;

namespace FoodOrder.Api.Features.Categories.CreateCategory;

public class CreateCategoryEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/categories", async (CreateCategoryRequest request, AppDbContext db) =>
        {
            var category = new Category
            {
                Name = request.Name
            };

            db.Categories.Add(category);
            await db.SaveChangesAsync();

            return Results.Created(
             $"/api/categories/{category.Id}",
             new CreateCategoryResponse(
                 category.Id,
                 category.Name
             )
            );
        })
        .RequireAuthorization("AdminOnly")
        .WithTags("Categories");
    }
}
