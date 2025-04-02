using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _entities;

    public Repository(AppDbContext context)
    {
        _context = context;
        _entities = context.Set<TEntity>();
    }

    public async Task<PagedResult<TEntity>> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 10,
        bool usePaging = true,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null
    )
    {
        var query = _entities.AsQueryable();
        if (include != null)
            query = include(query);

        int totalItems = await query.CountAsync().ConfigureAwait(false);
        List<TEntity> items;

        if (usePaging)
        {
            items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false);
        }
        else
        {
            items = await query.ToListAsync().ConfigureAwait(false);
            pageNumber = 1; // Khi không phân trang, pageNumber mặc định là 1
            pageSize = totalItems; // pageSize bằng tổng số bản ghi
        }

        return new PagedResult<TEntity>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize,
        };
    }

    public async Task<IEnumerable<TEntity>> FilterAsync(
        params (
            string PropertyName,
            object? Value,
            string? OtherPropertyName,
            string Comparison,
            LogicOperator? Logic
        )[] filters
    )
    {
        return await _entities.FilterByProperties(filters).ToListAsync().ConfigureAwait(false);
    }

    public async Task<TEntity?> GetByIdAsync(
        int id,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null
    )
    {
        IQueryable<TEntity> query = _entities.AsQueryable();

        if (include == null)
        {
            return await _entities.FindAsync(id).ConfigureAwait(false);
        }

        query = include(query);

        return await query
            .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id)
            .ConfigureAwait(false);
    }

    public virtual async Task<TEntity?> AddAsync(TEntity entity)
    {
        await _entities.AddAsync(entity).ConfigureAwait(false);
        var rowsAffected = await _context.SaveChangesAsync().ConfigureAwait(false);
        return rowsAffected > 0 ? entity : null;
    }

    public async Task<IEnumerable<TEntity>?> AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _entities.AddRangeAsync(entities).ConfigureAwait(false);
        var rowsAffected = await _context.SaveChangesAsync().ConfigureAwait(false);
        return rowsAffected > 0 ? entities : null;
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        _entities.Update(entity);
        return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    public async Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        _entities.UpdateRange(entities);
        return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _entities.FindAsync(id).ConfigureAwait(false);
        if (entity == null)
            return false;

        _entities.Remove(entity);
        return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync().ConfigureAwait(false);
    }
}
