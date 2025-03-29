public interface IProductRepository : IRepository<Product>
{
    Task<PagedResult<Product>> SearchProductByNameAsync(string name);
    Task<decimal> GetProductPriceByIdAsync(int productId);
    Task<PagedResult<Product>> GetSimilarProductsAsync(int productId,List<int> categoriesId);
    Task<decimal> GetTotalPriceByProductsIdAsync((int productId, int quantity)[] productsId);
    Task<List<string>> GetBrandsAsync();
}
