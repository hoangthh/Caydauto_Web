using Microsoft.AspNetCore.Mvc;
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
        
    }
    [HttpGet("all")]
    public async Task<IActionResult> GetProducts()
    {
        var products = await  _productService.GetProducts();
        return Ok(products);
    }
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] ProductFilter filter)
    {
        var products = await _productService.GetProducts(filter);
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
    public List<string>? Brands {get; set;}
    public string OrderBy { get; set; } = "Sold";
    public bool IsDesc { get; set; } = true;

}