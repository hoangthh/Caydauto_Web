using Microsoft.EntityFrameworkCore;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Cart> _dbSet;

    public CartRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<Cart>();
    }

    private Task<Cart?> GetCartByUserId(int userId)
    {
        return _dbSet
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .ThenInclude(p => p.Images)
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Color)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }
    public async Task<bool> AddToCart(int userId, CartItem cartItem)
    {
        var cart = await GetCartByUserId(userId).ConfigureAwait(false);
        // Kiểm tra xem giỏ hàng đã tồn tại chưa
        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                CartItems = new List<CartItem> { cartItem },
            };
            _dbSet.Add(cart);
        }
        else
        {
            var existingItem = cart.CartItems.FirstOrDefault(ci =>
                ci.ProductId == cartItem.ProductId && ci.ColorId == cartItem.ColorId
            );
            // Nếu sản phẩm đã tồn tại trong giỏ hàng, cập nhật số lượng
            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;
            }
            else
            {
                //Nếu không thì add vô giỏ hàng
                cart.CartItems.Add(cartItem);
            }
        }
        return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    public async Task<bool> ClearCart(int userId)
    {
        var cart = await GetCartByUserId(userId).ConfigureAwait(false);
        if (cart != null)
        {
            cart.CartItems.Clear();
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
        return false;
    }

    public async Task<Cart?> GetCart(int userId)
    {
        return await GetCartByUserId(userId).ConfigureAwait(false);
    }

    public async Task<bool> RemoveFromCart(int userId, int cartItemId)
    {
        var cart = await GetCartByUserId(userId).ConfigureAwait(false);
        if (cart != null)
        {
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem != null)
            {
                cart.CartItems.Remove(cartItem);
                return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
            }
        }
        return false;
    }
}
