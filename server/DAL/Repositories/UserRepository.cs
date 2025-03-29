using Microsoft.EntityFrameworkCore;

public class UserRepository : Repository<User>, IUserRepostory
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<User?> GetUserWithRoleByIdAsync(int id)
    {
        return await _entities
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
    }
}