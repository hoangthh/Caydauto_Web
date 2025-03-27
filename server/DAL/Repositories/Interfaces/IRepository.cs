using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<PagedResult<TEntity>> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 10,
        bool usePaging = true, // Thêm cờ usePaging
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null
    );
    Task<IEnumerable<TEntity>> FilterAsync(
        params (
            string PropertyName,
            object? Value,
            string? OtherPropertyName,
            string Comparison,
            LogicOperator? Logic
        )[] filters
    );

    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity?> AddAsync(TEntity entity); // Trả về null nếu thất bại
    Task<IEnumerable<TEntity>?> AddRangeAsync(IEnumerable<TEntity> entities); // Trả về null nếu thất bại
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(int id);
    Task<IDbContextTransaction> BeginTransactionAsync();
}
