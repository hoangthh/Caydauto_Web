using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context)
        : base(context) { }

    public Func<IQueryable<Order>, IQueryable<Order>> OrderNavigate(
        Expression<Func<Order, bool>>? predicate = null
    )
    {
        var query = _entities.IncludeMultiple(o => o.User, o => o.Discount);
        // Bao gồm OrderItems và các thực thể liên quan
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        return (
            q =>
                query
                    .Include(o => o.OrderItems)
                    .ThenInclude(p => p.Color)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Images)
        );
    }
}
