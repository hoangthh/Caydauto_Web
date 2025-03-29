using Microsoft.EntityFrameworkCore;
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

    public async Task<decimal> GetProductPriceByIdAsync(int productId)
    {
        var product = await GetByIdAsync(productId);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {productId} not found.");
        }
        return product.Price;
    }

    public async Task<PagedResult<Product>> GetSimilarProductsAsync(
        int productId,
        List<int> categoriesId
    )
    {
        return await GetAllAsync(
            pageNumber: 1,
            pageSize: 1,
            usePaging: true,
            include: q =>
                q.IncludeMultiple(q => q.Images, q => q.Categories)
                    .Where(p =>
                        p.Id != productId && p.Categories.Any(c => categoriesId.Contains(c.Id))
                    )
        );
    }

    public async Task<decimal> GetTotalPriceByProductsIdAsync(
        (int productId, int quantity)[] productsId
    )
    {
        Dictionary<int, decimal> productQuantities = new();
        var totalPrice = 0m;
        List<Tuple<int, decimal>> products = await _entities
            .AsNoTracking()
            .Where(p => productsId.Select(p => p.productId).Contains(p.Id))
            .Select(p => Tuple.Create(p.Id, p.Price))
            .ToListAsync();
        foreach (var product in products)
        {
            productQuantities.Add(product.Item1, product.Item2);
        }

        foreach (var product in productsId)
        {
            if (productQuantities.ContainsKey(product.productId))
            {
                totalPrice += productQuantities[product.productId] * product.quantity;
            }
        }

        return totalPrice;
    }
}
