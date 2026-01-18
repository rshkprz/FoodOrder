using System;
using FoodOrder.Api.Features.Categories.CreateCategory;
using FoodOrder.Api.Features.Categories.GetCategories;

namespace FoodOrder.Api.Features.Categories;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        CreateCategoryEndpoint.Map(app);
        GetCategoriesEndpoint.Map(app);
    }
}
