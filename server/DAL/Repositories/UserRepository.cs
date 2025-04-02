using Microsoft.EntityFrameworkCore;

public class UserRepository : Repository<User>, IUserRepostory
{
    public UserRepository(AppDbContext context)
        : base(context) { }

    public async Task<User?> GetUserWithRoleByIdAsync(int id)
    {
        return await _entities
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id)
            .ConfigureAwait(false);
    }

    public async Task<bool> AddProductIntoWishList(User user, Product product)
    {
        user.WishList.Add(product);
        return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    public async Task<bool> RemoveProductFromWishList(User user, Product product)
    {
        user.WishList.Remove(product);
        return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }
}
