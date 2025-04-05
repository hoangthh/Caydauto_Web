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

    public async Task<int> GetProductPriceByIdAsync(int productId)
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

    public async Task<int> GetTotalPriceByProductsIdAsync(
        (int ProductId, int Quantity)[] productsId
    )
    {
        // Tạo dictionary từ productsId để tra cứu nhanh số lượng
        var quantityDict = productsId.ToDictionary(p => p.ProductId, p => p.Quantity);

        // Lấy giá sản phẩm từ database
        var productPrices = await _entities
            .AsNoTracking()
            .Where(p => productsId.Select(x => x.ProductId).Contains(p.Id))
            .Select(p => new { p.Id, p.Price })
            .ToDictionaryAsync(p => p.Id, p => p.Price)
            .ConfigureAwait(false);

        // Tính tổng giá
        int totalPrice = 0;
        foreach (var productId in quantityDict.Keys)
        {
            if (productPrices.TryGetValue(productId, out var price))
            {
                totalPrice += price * quantityDict[productId];
            }
            else
            {
                throw new Exception($"Product with ID {productId} not found.");
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
        productsId = productsId
            .GroupBy(p => p.productId) // Nhóm theo productId
            .Select(g => (productId: g.Key, quantity: g.Sum(p => p.quantity))) // Cộng dồn quantity cho mỗi nhóm
            .ToArray();
        // Tạo dictionary từ productsId để tra cứu nhanh số lượng
        var quantityDict = productsId.ToDictionary(p => p.productId, p => p.quantity);
        var missingProducts = quantityDict
            .Keys.Except(productsId.Select(p => p.productId))
            .ToList();
        if (missingProducts.Any())
        {
            throw new Exception(
                $"Products with IDs {string.Join(", ", missingProducts)} not found."
            );
        }
        // Lấy thông tin sản phẩm từ database
        var products = await _entities
            .AsNoTracking()
            .Where(p => productsId.Select(x => x.productId).Contains(p.Id))
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.StockQuantity,
            })
            .ToListAsync()
            .ConfigureAwait(false);

        // Kiểm tra số lượng và trả về danh sách sản phẩm không đủ
        return products
            .Where(p => quantityDict.ContainsKey(p.Id) && p.StockQuantity < quantityDict[p.Id])
            .Select(p => p.Name)
            .ToList();
    }

    public async Task<Dictionary<int, int>> GetProductPriceByIdsAsync(int[] productIds)
    {
        // Lấy danh sách sản phẩm với giá
        var products = await _entities
            .AsNoTracking()
            .Where(p => productIds.Contains(p.Id))
            .Select(p => new { p.Id, p.Price })
            .ToListAsync()
            .ConfigureAwait(false);

        // Sử dụng Distinct để loại bỏ trùng lặp dựa trên Id
        var distinctProducts = products
            .GroupBy(p => p.Id) // Nhóm các sản phẩm theo Id
            .Select(g => g.First()) // Lấy phần tử đầu tiên của mỗi nhóm
            .ToList();

        var prices = distinctProducts.ToDictionary(p => p.Id, p => p.Price);
        // Kiểm tra xem có sản phẩm nào không tìm thấy không
        var missingIds = productIds.Except(prices.Keys).ToList();
        if (missingIds.Any())
        {
            throw new Exception($"Products with IDs {string.Join(", ", missingIds)} not found.");
        }

        return prices;
    }
}
