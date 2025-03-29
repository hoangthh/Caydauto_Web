using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Role> GetUserRole()
    {
        return await _entities.FirstOrDefaultAsync(r => r.Name == "User").ConfigureAwait(false) ?? new Role{Name = "User"};
    }

    public async Task<Role> GetAdminRole()
    {
        return await _entities.FirstOrDefaultAsync(r => r.Name == "Admin").ConfigureAwait(false) ?? new Role{Name = "Admin"};
    }
}