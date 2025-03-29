using System.ComponentModel.DataAnnotations;

public class CartItemGetDto
{
    public int Id { get; set; }
    public CartProdutGetDto Product { get; set; } = new CartProdutGetDto();
    public ColorGetDto Color { get; set; } = new ColorGetDto();
    public int Quantity { get; set; }
}
public class CartProdutGetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = Constraint.Image.DefaultProductImageUrl;
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

