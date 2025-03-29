using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{

    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _entities.AsNoTracking().ToListAsync();
    }
}