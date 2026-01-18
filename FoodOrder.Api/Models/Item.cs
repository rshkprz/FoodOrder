using System;
using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Api.Models;

public class Item
{
    public Guid Id { get; set; }
    [Required]
    public Guid CategoryId { get; set; }
    [Required, MaxLength(150)]
    public string Name { get; set; } = default!;
    public string? ImageUrl { get; set; }
    [Range(0, 100000)]
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Category category { get; set; } = default!;
}
