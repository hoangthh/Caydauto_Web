using System.ComponentModel.DataAnnotations;

public class ProductGetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "Blank product";
    public decimal Price { get; set; }
    public string Description { get; set; } = "This is a blank product";
    public string Brand { get; set; } = "Blank brand";
    public int StockQuantity { get; set; }
    public double AverageRating { get; set; }
}

public class ProductCreateDto
{
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
    public List<ImageCreateDto> Images { get; set; } = new List<ImageCreateDto>();
}

public class ProductPutDto
{
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
    public List<ImagePutDto> Images { get; set; } = new List<ImagePutDto>();
}
