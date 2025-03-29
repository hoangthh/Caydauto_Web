public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
}