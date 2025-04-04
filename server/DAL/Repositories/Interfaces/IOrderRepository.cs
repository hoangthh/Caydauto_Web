using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

public interface IOrderRepository : IRepository<Order>
{
    Func<IQueryable<Order>, IQueryable<Order>> OrderNavigate(Expression<Func<Order, bool>>? predicate = null);
}


