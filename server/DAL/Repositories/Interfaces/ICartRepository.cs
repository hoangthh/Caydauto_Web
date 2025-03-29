public interface ICartRepository {
    public Task<bool> AddToCart(int userId, CartItem cartItem);
    public Task<bool> RemoveFromCart(int userId, int cartItemId);
    public Task<bool> ClearCart(int userId);
    public Task<Cart?> GetCart(int userId);

}