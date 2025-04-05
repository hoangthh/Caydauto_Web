using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DiscountController : ControllerBase
{
    private readonly IDiscountService _discountService;

    public DiscountController(IDiscountService discountService)
    {
        _discountService = discountService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllDiscounts([FromQuery] int pageNumber, int pageSize)
    {
        var discounts = await _discountService.GetDiscountsAsync(pageNumber, pageSize).ConfigureAwait(false);
        return Ok(discounts.discounts);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDiscountById(int id)
    {
        var discount = await _discountService.GetDiscountByIdAsync(id).ConfigureAwait(false);
        if (!discount.Success)
        {
            return BadRequest(discount.Message);
        }
        return Ok(discount.discount);
    }
    [HttpGet("code/{code}")]
    public async Task<IActionResult> GetDiscountByCode(string code)
    {
        var discount = await _discountService.GetDiscountByCodeAsync(code).ConfigureAwait(false);
        if (!discount.Success)
        {
            return BadRequest(discount.Message);
        }
        return Ok(discount.discount);
    }
    [HttpPost]
    public async Task<IActionResult> CreateDiscount([FromBody] DiscountCreateDto discountCreateDto)
    {
        var discount = await _discountService.CreateDiscountAsync(discountCreateDto).ConfigureAwait(false);
        if (!discount.Success)
        {
            return BadRequest(discount.Message);
        }
        return Ok(discount.Message);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDiscount(int id, [FromBody] DiscountPutDto discountUpdateDto)
    {
        var discount = await _discountService.UpdateDiscountAsync(id, discountUpdateDto).ConfigureAwait(false);
        if (!discount.Success)
        {
            return BadRequest(discount.Message);
        }
        return Ok(discount.Message);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiscount(int id)
    {
        var discount = await _discountService.DeleteDiscountAsync(id).ConfigureAwait(false);
        if (!discount.Success)
        {
            return BadRequest(discount.Message);
        }
        return Ok(discount.Message);
    }
}
