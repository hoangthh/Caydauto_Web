using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name is required")]
    [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
    public string Name { get; set; } = "Blank category";

    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
    public string Description { get; set; } = "This is a blank category";

    // Navigation property
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
