using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Color
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Color name is required")]
    [StringLength(20, ErrorMessage = "Color name cannot exceed 20 characters")]
    public string Name { get; set; } = string.Empty;

    // Navigation property
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
