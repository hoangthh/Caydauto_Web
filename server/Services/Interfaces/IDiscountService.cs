using AutoMapper;

public interface IDiscountService
{
    Task<(bool Success, string Message, PagedResult<DiscountGetDto>? discounts)> GetDiscountsAsync(int pageNumber, int pageSize);
    Task<(bool Success, string Message, DiscountGetDto? discount)> GetDiscountByIdAsync(int id);
    Task<(bool Success, string Message)> CreateDiscountAsync(DiscountCreateDto discountCreateDto);
    Task<(bool Success, string Message)> UpdateDiscountAsync(
        int id,
        DiscountPutDto discountUpdateDto
    );
    Task<(bool Success, string Message)> DeleteDiscountAsync(int id);
    Task<(bool Success, string Message, DiscountGetDto? discount)> GetDiscountByCodeAsync(string code);
}

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public DiscountService(IDiscountRepository discountRepository, IMapper mapper)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    public async Task<(bool Success, string Message, PagedResult<DiscountGetDto>? discounts)> GetDiscountsAsync(int pageNumber, int pageSize)
    {
        var discounts = await _discountRepository
            .GetAllDiscountsAsync(pageNumber, pageSize)
            .ConfigureAwait(false);
        if (discounts == null || !discounts.Any())
            return (false, "No discounts found", null);
        var discountDtos = _mapper.Map<List<DiscountGetDto>>(discounts.ToList());
        var pagedResult = new PagedResult<DiscountGetDto>
        {
            Items = discountDtos,
            TotalItems = discounts.Count(),
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        return (true, "Get discounts successfully", pagedResult);
    }

    public async Task<(
        bool Success,
        string Message,
        DiscountGetDto? discount
    )> GetDiscountByIdAsync(int id)
    {
        var discount = await _discountRepository.GetByIdAsync(id).ConfigureAwait(false);
        if (discount == null)
            return (false, "Discount not found", null);
        if (discount.Quantity <= 0)
            return (false, "Discount code is expired", null);
        var discountDto = _mapper.Map<DiscountGetDto>(discount);
        return (true, "Get discount successfully", null);
    }

    public async Task<(
        bool Success,
        string Message,
        DiscountGetDto? discount
    )> GetDiscountByCodeAsync(string code)
    {
        var discount = await _discountRepository.GetDiscountByCodeAsync(code).ConfigureAwait(false);
        if (discount == null)
            return (false, "Discount not found", null);
        if (discount.Quantity <= 0)
            return (false, "Discount code is expired", null);
        var discountDto = _mapper.Map<DiscountGetDto>(discount);
        return (true, "Get discount successfully", discountDto);
    }

    public async Task<(bool Success, string Message)> CreateDiscountAsync(
        DiscountCreateDto discountCreateDto
    )
    {
        var discount = _mapper.Map<Discount>(discountCreateDto);
        await _discountRepository.AddAsync(discount).ConfigureAwait(false);
        return (true, "Create discount successfully");
    }

    public async Task<(bool Success, string Message)> UpdateDiscountAsync(
        int id,
        DiscountPutDto discountUpdateDto
    )
    {
        var discount = await _discountRepository.GetByIdAsync(id).ConfigureAwait(false);
        if (discount == null)
            return (false, "Discount not found");
        _mapper.Map(discountUpdateDto, discount);
        await _discountRepository.UpdateAsync(discount).ConfigureAwait(false);
        return (true, "Update discount successfully");
    }

    public async Task<(bool Success, string Message)> DeleteDiscountAsync(int id)
    {
        var discount = await _discountRepository.GetByIdAsync(id).ConfigureAwait(false);
        if (discount == null)
            return (false, "Discount not found");
        await _discountRepository.DeleteAsync(id).ConfigureAwait(false);
        return (true, "Delete discount successfully");
    }
}
