using Microsoft.EntityFrameworkCore.Query;

public class ProductRepository(AppDbContext context)
    : Repository<Product>(context),
        IProductRepository
{
    public async Task<PagedResult<Product>> SearchProductByNameAsync(string name)
    {
        return await GetAllAsync(
                usePaging: true,
                include: q =>
                    (IIncludableQueryable<Product, object>)
                        q.IncludeMultiple(q => q.Categories, q => q.Colors, q => q.Images)
                        .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            );
            
    }
}
