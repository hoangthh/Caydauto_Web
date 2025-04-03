using System.Threading.Tasks;
using Microsoft.Identity.Client;

public interface IAddressService
{
    Task<(bool Success, string Message)> GetAddress(
        string shippingAddress,
        int provinceId,
        int districtId,
        string WardCode
    );
}

public class AddressService : IAddressService
{
    private readonly IDeliveryService _deliveryService;

    public AddressService(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    public async Task<(bool Success, string Message)> GetAddress(
        string shippingAddress,
        int provinceId,
        int districtId,
        string WardCode
    )
    {
        var provinces = await _deliveryService.GetProvincesAsync().ConfigureAwait(false);
        if (provinces == null)
            return (false, "Can't get provinces");
        var province = provinces.FirstOrDefault(p => p.ProvinceID == provinceId);
        if (province == null)
            return (false, "Not Exist this province");
        var districts = await _deliveryService.GetDistrictsAsync(provinceId).ConfigureAwait(false);
        if (districts == null)
            return (false, "Can't get districts");
        var district = districts.FirstOrDefault(d => d.DistrictID == districtId);
        if (district == null)
            return (false, "Not Exist this district");
        var wards = await _deliveryService.GetWardsAsync(districtId).ConfigureAwait(false);
        if (wards == null)
            return (false, "Can't get wards");
        var ward = wards.FirstOrDefault(w => w.WardCode == WardCode);
        if (ward == null)
            return (false, "Not Exist this ward");
        return (
            true,
            $"{province.ProvinceName} - {district.DistrictName} - {ward.WardName} - {shippingAddress}"
        );
    }
}
