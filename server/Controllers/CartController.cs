using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        try
        {
            var cart = await _cartService.GetCart().ConfigureAwait(false);
            return Ok(cart);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddItemToCart([FromBody] CartItemCreateDto item)
    {
        try
        {
            if (item == null)
                return BadRequest(new { Message = "Item cannot be null" });
            var result = await _cartService.AddToCart(item).ConfigureAwait(false);
            if (!result)
                return BadRequest(new { Message = "Failed to add item to cart" });
            return Ok(new { Message = "Item added to cart successfully" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpDelete("remove/{cartItemId}")]
    public async Task<IActionResult> RemoveItemFromCart(int cartItemId)
    {
        try
        {
            var result = await _cartService.RemoveFromCart(cartItemId).ConfigureAwait(false);
            if (!result)
                return BadRequest(new { Message = "Failed to remove item from cart" });
            return Ok(new { Message = "Item removed from cart successfully" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearCart()
    {
        try
        {
            var result = await _cartService.ClearCart().ConfigureAwait(false);
            if (!result)
                return BadRequest(new { Message = "Failed to clear cart" });
            return Ok(new { Message = "Cart cleared successfully" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
