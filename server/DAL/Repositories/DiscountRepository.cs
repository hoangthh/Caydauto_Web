using Microsoft.EntityFrameworkCore;

public class DiscountRepository : Repository<Discount>, IDiscountRepository
{
    public DiscountRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Discount?> GetDiscountByCodeAsync(string code)
    {
        return await _context.Discounts
            .FirstOrDefaultAsync(d => d.Code == code).ConfigureAwait(false);
    }
    public async Task<IEnumerable<Discount>> GetAllDiscountsAsync(int pageNumber, int pageSize)
    {
        return await _context.Discounts
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync().ConfigureAwait(false);
    }

    override public async Task<Discount?> AddAsync(Discount entity)
    {
        var existingDiscount = await _context.Discounts
            .FirstOrDefaultAsync(d => d.Code == entity.Code).ConfigureAwait(false);
        if (existingDiscount != null)
        {
            existingDiscount.Value += entity.Value;
            var result = await UpdateAsync(existingDiscount).ConfigureAwait(false);
            return existingDiscount;
        }
        return await base.AddAsync(entity).ConfigureAwait(false);
    }
}