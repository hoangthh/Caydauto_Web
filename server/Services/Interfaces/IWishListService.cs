using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

public interface IWishListService
{
    Task<WishListResponse> AddProductToWishList(int productId);
    Task<PagedResult<WishListItemDto>> GetUserWishList();
    Task<bool> RemoveProductFromWishList(int productId);
    Task<bool> MarkProductsWishList(int userId, List<ProductAllGetDto> products);
}
