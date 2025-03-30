public interface IOrderRepository : IRepository<Order> {

}

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }

    
    
}