using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class DeliveryController : ControllerBase
{
    private readonly IDeliveryService _deliveryService;

    public DeliveryController(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    [HttpGet("provinces")]
    public async Task<IActionResult> GetProvinces()
    {
        return Ok(await _deliveryService.GetProvincesAsync());
    }
    [HttpGet("districts/{provinceId}")]
    public async Task<IActionResult> GetDistricts(int provinceId)
    {
        return Ok(await _deliveryService.GetDistrictsAsync(provinceId));
    }
    [HttpGet("wards/{districtId}")]
    public async Task<IActionResult> GetWards(int districtId)
    {
        return Ok(await _deliveryService.GetWardsAsync(districtId));
    }
    [HttpGet("services/{fromDistrictId}/{toDistrictId}")]
    public async Task<IActionResult> GetServices(int fromDistrictId, int toDistrictId)
    {
        return Ok(await _deliveryService.GetAvailableServicesAsync(fromDistrictId, toDistrictId));
    }
    [HttpGet("shipping-fee")]
    public async Task<IActionResult> GetShippingFee(int toDistrictId, string toWardCode, int insuranceValue)
    {
        return Ok(await _deliveryService.GetShippingFeeAsync(toDistrictId, toWardCode, insuranceValue));
    }

}
