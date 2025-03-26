using System.ComponentModel.DataAnnotations;

public class OrderItemGetDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string OrderItemUrl { get; set; } = Constraint.Image.DefaultProductImageUrl;
}

public class OrderItemCreateDto
{
    [Required(ErrorMessage = "Product ID is required")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Color ID is required")]
    public int ColorId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Unit price is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Unit price must be greater than or equal to 0")]
    public decimal UnitPrice { get; set; }
}

