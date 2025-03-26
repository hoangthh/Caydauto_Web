public interface IProductRepository : IRepository<Product>
{
    Task<PagedResult<Product>> SearchProductByNameAsync(string name);
}
