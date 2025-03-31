using System.Collections;
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
            )
            .ConfigureAwait(false);
    }

    public async Task<List<Product>> GetByIdsAsync(IEnumerable<int> productIds)
    {
        return await _context
            .Products.Where(p => productIds.Contains(p.Id))
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<decimal> GetProductPriceByIdAsync(int productId)
    {
        var product = await GetByIdAsync(productId).ConfigureAwait(false);
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
            )
            .ConfigureAwait(false);
    }

    public async Task<decimal> GetTotalPriceByProductsIdAsync(
        (int productId, int quantity)[] productsId
    )
    {
        Dictionary<int, decimal> productPrices = new();
        var totalPrice = 0m;
        List<Tuple<int, decimal>> products = await _entities
            .AsNoTracking()
            .Where(p => productsId.Select(p => p.productId).Contains(p.Id))
            .Select(p => Tuple.Create(p.Id, p.Price))
            .ToListAsync()
            .ConfigureAwait(false);
        foreach (var product in products)
        {
            productPrices.Add(product.Item1, product.Item2);
        }

        foreach (var product in productsId)
        {
            if (productPrices.ContainsKey(product.productId))
            {
                totalPrice += productPrices[product.productId] * product.quantity;
            }
        }

        return totalPrice;
    }

    public async Task<List<string>> GetBrandsAsync()
    {
        return await _entities
            .AsNoTracking()
            .Select(p => p.Brand)
            .Distinct()
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<string>> CheckQuantityProducts(
        (int productId, int quantity)[] productsId
    )
    {
        Dictionary<int, (string, int)> productQuantities = new();
        Tuple<int, string, int>[] products = await _entities
            .AsNoTracking()
            .Where(p => productsId.Select(p => p.productId).Contains(p.Id))
            .Select(p => Tuple.Create(p.Id, p.Name, p.StockQuantity))
            .ToArrayAsync()
            .ConfigureAwait(false);
        foreach (var product in products)
        {
            productQuantities.Add(product.Item1, (product.Item2, product.Item3));
        }
        var productsNotQualified = new List<string>();
        foreach (var product in productsId)
        {
            if (
                productQuantities.ContainsKey(product.productId)
                && productQuantities[product.productId].Item2 < product.quantity
            )
            {
                productsNotQualified.Add(productQuantities[product.productId].Item1);
            }
        }
        return productsNotQualified;
    }

    public async Task<Dictionary<int, decimal>> GetProductPriceByIdsAsync(int[] productIds)
    {
        var product = await _entities
            .Where(p => productIds.Contains(p.Id))
            .ToArrayAsync()
            .ConfigureAwait(false);
        return product.Select(p => new { p.Id, p.Price }).ToDictionary(p => p.Id, p => p.Price);
    }
}
