using System.ComponentModel.DataAnnotations;

public class CartItemGetDto
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public int Quantity { get; set; }
}

public class CartItemCreateDto
{
    [Required(ErrorMessage = "Product ID is required")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Color ID is required")]
    public int ColorId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }
}

