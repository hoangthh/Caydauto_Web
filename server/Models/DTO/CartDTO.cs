using System.ComponentModel.DataAnnotations;

public class CartGetDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<CartItemGetDto> CartItems { get; set; } = new List<CartItemGetDto>();
}

public class CartCreateDto
{
    public CartItemCreateDto CartItem { get; set; } = new CartItemCreateDto();
}
