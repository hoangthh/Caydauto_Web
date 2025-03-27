using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class Product : IDateTracking
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required")]
    [StringLength(
        100,
        MinimumLength = 2,
        ErrorMessage = "Product name must be between 2 and 100 characters"
    )]
    public string Name { get; set; } = "Blank product";

    [Required(ErrorMessage = "Price is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
    public decimal Price { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = "This is a blank product";

    [Required(ErrorMessage = "Brand is required")]
    [StringLength(50, ErrorMessage = "Brand cannot exceed 50 characters")]
    public string Brand { get; set; } = "Blank brand";

    [Required(ErrorMessage = "Stock quantity is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be greater than or equal to 0")]
    public int StockQuantity { get; set; }
    public int Sold { get; set; } = 0;
    public double AverageRating => Comments.Any() ? Comments.Average(c => c.Rating) : 0;

    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime UpdatedDate {get; set;}
    // Navigation properties
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Color> Colors { get; set; } = new List<Color>();
    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<User> WishListedByUsers { get; set; } = new List<User>();
}
