using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ColorController : ControllerBase
{
    private readonly IColorService _colorService;

    public ColorController(IColorService colorService)
    {
        _colorService = colorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllColors()
    {
        var colors = await _colorService.GetAllColorsAsync().ConfigureAwait(false);
        return Ok(colors);
    }

    [HttpPost]
    public async Task<IActionResult> CreateColor([FromBody] ColorCreateDto color)
    {
        try
        {
            if (color == null)
            {
                return BadRequest("Color cannot be null.");
            }

            var createdColor = await _colorService.CreateColorAsync(color).ConfigureAwait(false);
            return Ok(createdColor);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateColor(int id, [FromBody] ColorPutDto color)
    {
        try
        {
            var result = await _colorService.UpdateColorAsync(id, color).ConfigureAwait(false);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
