using Microsoft.EntityFrameworkCore;

public class DiscountRepository : Repository<Discount>, IDiscountRepository
{
    public DiscountRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Discount?> GetDiscountByCodeAsync(string code)
    {
        return await _context.Discounts
            .FirstOrDefaultAsync(d => d.Code == code);
    }
    public async Task<IEnumerable<Discount>> GetAllDiscountsAsync(int pageNumber, int pageSize)
    {
        return await _context.Discounts
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

}