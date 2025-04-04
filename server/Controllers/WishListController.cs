using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WishListController : ControllerBase
{
    private readonly IWishListService _wishListService;

    public WishListController(IWishListService wishListService)
    {
        _wishListService = wishListService;
    }

    [HttpGet]
    public async Task<IActionResult> GetWishList()
    {
        return Ok(await _wishListService.GetUserWishList().ConfigureAwait(false));
    }

    [HttpPost("{productId}")]
    public async Task<IActionResult> AddWishList(int productId)
    {
        var result = await _wishListService.AddProductToWishList(productId).ConfigureAwait(false);
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return Ok(result.wishListItemDto);
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveWishList(int productId)
    {
        var result = await _wishListService
            .RemoveProductFromWishList(productId)
            .ConfigureAwait(false);
        if (!result)
        {
            return BadRequest("Cannot Remove from WishList");
        }
        return Ok($"Product {productId} Removed from WishList");
    }
}
