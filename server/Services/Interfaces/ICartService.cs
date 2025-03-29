public interface ICartService {

    public Task<bool> AddToCart(CartItemCreateDto cartItem);
    public Task<bool> RemoveFromCart(int cartItemId);
    public Task<bool> ClearCart();
    public Task<CartGetDto> GetCart();
}