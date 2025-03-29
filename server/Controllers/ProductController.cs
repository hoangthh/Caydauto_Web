using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IColorService _colorService;

    public ProductController(
        IProductService productService,
        ICategoryService categoryService,
        IColorService colorService
    )
    {
        _productService = productService;
        _categoryService = categoryService;
        _colorService = colorService;
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetFilter()
    {
        var categories = await _categoryService.GetAllCategoriesAsync().ConfigureAwait(false);
        var colors = await _colorService.GetAllColorsAsync().ConfigureAwait(false);
        var brands = await _productService.GetBrands().ConfigureAwait(false);
        return Ok(
            new
            {
                Categories = categories.Select(c => new { c.Id, c.Name }),
                Colors = colors.Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.HexCode,
                }),
                Brands = brands,
                Prices = SeedData
                    .PriceRanges.Select(p => new
                    {
                        Label = p.Key,
                        Min = p.Value.Min * 1000,
                        Max = p.Value.Max * 1000,
                    })
                    .ToList(),
            }
        );
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetProducts(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
            return BadRequest(new { Message = "Page and PageSize must be greater than 0." });
        if (pageSize > 100)
            return BadRequest(new { Message = "PageSize must be less than or equal to 100." });
        var products = await _productService.GetProducts(page, pageSize).ConfigureAwait(false);
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> GetProducts([FromBody] ProductFilter filter)
    {
        var products = await _productService.GetProducts(filter).ConfigureAwait(false);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _productService.GetProduct(id).ConfigureAwait(false);
        if (product == null)
            return NotFound(
                new { Message = $"Product with the specified ID = {id} was not found." }
            );
        return Ok(product);
    }

    [HttpGet("similar/{id}")]
    public async Task<IActionResult> GetSimilarProducts(int id)
    {
        var products = await _productService.GetSimilarProducts(id).ConfigureAwait(false);
        return Ok(products);
    }
}

public class ProductFilter
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 6;
    public string? Name { get; set; }
    public List<int>? Categories { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public List<int>? Colors { get; set; }
    public List<string>? Brands { get; set; }
    public string OrderBy { get; set; } = "Sold";
    public bool IsDesc { get; set; } = true;
}
