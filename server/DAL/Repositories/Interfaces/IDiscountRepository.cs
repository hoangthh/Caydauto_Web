public interface IDiscountRepository : IRepository<Discount>
{
    Task<Discount?> GetDiscountByCodeAsync(string code);
    Task<IEnumerable<Discount>> GetAllDiscountsAsync(int pageNumber, int pageSize);
}