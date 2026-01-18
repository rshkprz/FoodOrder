namespace FoodOrder.Api.Features.Categories.CreateCategory;

public record CreateCategoryRequest(
    string Name
);

public record CreateCategoryResponse(
    Guid Id,
    string Name
);

