using System.ComponentModel.DataAnnotations;

public class OrderItemGetDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public ColorGetDto Color { get; set; } = new ColorGetDto();
    public OrderItemProdutGetDto Product { get; set; } = new OrderItemProdutGetDto();
    public int Quantity { get; set; }
    public int UnitPrice { get; set; }
}
public class OrderItemProdutGetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
    public string ImageUrl { get; set; } = Constraint.Image.DefaultProductImageUrl;
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

}

