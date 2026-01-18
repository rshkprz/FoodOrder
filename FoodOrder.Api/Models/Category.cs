using System;
using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Api.Models;

public class Category
{
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = default!;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
