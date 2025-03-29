using Microsoft.EntityFrameworkCore;

public class ColorRepository : Repository<Color>, IColorRepository
{
    public ColorRepository(AppDbContext context)
        : base(context) { }

    public async Task<IEnumerable<Color>> GetAllAsync()
    {
        return await _entities.AsNoTracking().ToListAsync().ConfigureAwait(false);
    }

    public async Task<bool> IsColorCreatedAsync(string colorName, int? id = null)
    {
        return await _entities.AsNoTracking()
            .AnyAsync(c => c.Name == colorName && c.Id != id).ConfigureAwait(false);
    }
}
