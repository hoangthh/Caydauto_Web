using System.ComponentModel.DataAnnotations;

public class CartItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Cart ID is required")]
    public int CartId { get; set; }

    [Required(ErrorMessage = "Product ID is required")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Color ID is required")]
    public int ColorId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }

    // Navigation properties
    public Cart Cart { get; set; }
    public Product Product { get; set; } = new Product();
    public Color Color { get; set; } = new Color();
}
