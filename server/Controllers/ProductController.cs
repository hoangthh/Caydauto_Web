using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetProducts(int page, int pageSize)
    {
        var products = await _productService.GetProducts(page, pageSize);
        return Ok(products);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] ProductFilter filter)
    {
        var products = await _productService.GetProducts(filter);
        return Ok(products);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _productService.GetProduct(id);
        if (product == null)
            return NotFound(new { Message = $"Product with the specified ID = {id} was not found." });
        return Ok(product);
    }
    [HttpGet("similar/{id}")]
    public async Task<IActionResult> GetSimilarProducts(int id)
    {
        var products = await _productService.GetSimilarProducts(id);
        return Ok(products);
    }
}

public class ProductFilter
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 6;
    public string? Name { get; set; }
    public List<string>? Categories { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public List<string>? Colors { get; set; }
    public List<string>? Brands { get; set; }
    public string OrderBy { get; set; } = "Sold";
    public bool IsDesc { get; set; } = true;
}
