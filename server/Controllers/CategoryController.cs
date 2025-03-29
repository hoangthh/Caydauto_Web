using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync().ConfigureAwait(false);
        if (categories == null || !categories.Any())
            return NotFound("No categories found.");
        return Ok(categories);
    }
    [HttpPost("create")]
    public async Task<IActionResult> CreateCategory([FromBody]CategoryCreateDto category)
    {
        var result = await _categoryService.CreateCategoryAsync(category)
            .ConfigureAwait(false);
        if (result == null)
            return BadRequest("Failed to create category.");
        return Ok(result);
    }
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryPutDto category)
    {
        var result = await _categoryService.UpdateCategoryAsync(id, category)
            .ConfigureAwait(false);
        if (!result)
            return NotFound("Category not found.");
        return Ok("Category updated successfully.");
    }
}