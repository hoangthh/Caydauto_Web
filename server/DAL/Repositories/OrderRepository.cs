using Microsoft.EntityFrameworkCore;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context)
        : base(context) { }

    public Func<IQueryable<Order>, IQueryable<Order>> OrderNavigate()
    {
        var query = _entities.IncludeMultiple(o => o.User, o => o.Discount);

        // Bao gồm OrderItems và các thực thể liên quan
        return (
            q =>
                query
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Colors)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Images)
        );
    }
}
