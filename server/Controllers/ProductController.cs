using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IColorService _colorService;
    private readonly IDistributedCache _cache;
    private readonly ILogger<ProductController> _logger;

    public ProductController(
        IProductService productService,
        ICategoryService categoryService,
        IColorService colorService,
        IDistributedCache cache,
        ILogger<ProductController> logger
    )
    {
        _productService = productService;
        _categoryService = categoryService;
        _colorService = colorService;
        _cache = cache;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateProductAsync([FromBody] ProductCreateDto product)
    {
        try
        {
            var result = await _productService.AddProduct(product).ConfigureAwait(false);
            if (result == null)
                return BadRequest(new { Message = "Failed to create product." });
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            return StatusCode(500, new { Message = "Internal server error" });
        }
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetFilter()
    {
        const string cacheKey = "filterData";
        // Kiểm tra dữ liệu trong cache
        var cachedData = await _cache.GetStringAsync(cacheKey).ConfigureAwait(false);
        if (!string.IsNullOrEmpty(cachedData))
        {
            var filterData = JsonConvert.DeserializeObject<FilterData>(cachedData);
            return Ok(filterData);
        }

        // Lấy dữ liệu từ các service nếu cache trống
        var categories = await _categoryService.GetAllCategoriesAsync().ConfigureAwait(false);
        var colors = await _colorService.GetAllColorsAsync().ConfigureAwait(false);
        var brands = await _productService.GetBrands().ConfigureAwait(false);

        var filterDataObj = new FilterData
        {
            Categories = categories
                .Select(c => new FilterData.Category { Id = c.Id.ToString(), Name = c.Name })
                .ToList(),
            Colors = colors
                .Select(c => new FilterData.Color
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    HexCode = c.HexCode,
                })
                .ToList(),
            Brands = brands.ToList(),
            Prices = SeedData
                .PriceRanges.Select(pr => new FilterData.Price
                {
                    Label = pr.Key,
                    Min = pr.Value.Min,
                    Max = pr.Value.Max,
                })
                .ToList(),
        };
        // Serialize dữ liệu và lưu vào cache với thời gian sống (ví dụ 30 phút)
        var serializedData = JsonConvert.SerializeObject(filterDataObj);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30),
        };
        await _cache.SetStringAsync(cacheKey, serializedData, options).ConfigureAwait(false);

        return Ok(filterDataObj);
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
    public string? Name { get; set; } = string.Empty;
    public List<int>? Categories { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public List<int>? Colors { get; set; }
    public List<string>? Brands { get; set; }
    public string OrderBy { get; set; } = "Sold";
    public bool IsDesc { get; set; } = true;
}

public class FilterData
{
    public class Category
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }

    public class Color
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? HexCode { get; set; }
    }

    public class Price
    {
        public string? Label { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
    }

    public List<Category>? Categories { get; set; }
    public List<Color>? Colors { get; set; }
    public List<string>? Brands { get; set; }
    public List<Price>? Prices { get; set; }
}
