using System.ComponentModel.DataAnnotations;

public class OrderItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Order ID is required")]
    public int OrderId { get; set; }

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
    
    // Navigation properties
    public Order Order { get; set; } = new Order();
    public Product Product { get; set; } = new Product();
    public Color Color { get; set; } = new Color();
}
