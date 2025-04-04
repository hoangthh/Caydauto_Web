using System.Collections;

public interface IProductRepository : IRepository<Product>
{
    Task<PagedResult<Product>> SearchProductByNameAsync(string name);
    Task<int> GetProductPriceByIdAsync(int productId);
    Task<PagedResult<Product>> GetSimilarProductsAsync(int productId, List<int> categoriesId);
    Task<int> GetTotalPriceByProductsIdAsync((int ProductId, int Quantity)[] productsId);
    Task<List<string>> GetBrandsAsync();
    Task<List<Product>> GetByIdsAsync(IEnumerable<int> productIds);
    Task<IEnumerable<string>> CheckQuantityProducts((int productId, int quantity)[] productsId);
    Task<Dictionary<int, int>> GetProductPriceByIdsAsync(int[] productIds);
    //IQueryable<Product> BuildQuery(ProductFilter filter);
}
