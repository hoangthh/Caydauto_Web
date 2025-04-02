using Microsoft.EntityFrameworkCore;

public interface IOrderRepository : IRepository<Order>
{
    Func<IQueryable<Order>, IQueryable<Order>> OrderNavigate();
}


