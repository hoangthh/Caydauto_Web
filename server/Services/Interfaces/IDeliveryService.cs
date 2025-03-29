public interface IDeliveryService
{
    Task<string> GetProvincesAsync();
    Task<string> GetDistrictsAsync(int provinceId);
    Task<string> GetWardsAsync(int districtId);
    Task<string> GetAvailableServicesAsync(int fromDistrictId, int toDistrictId);
    Task<string> GetShippingFeeAsync(
        int toDistrictId,
        string toWardCode,
        int insuranceValue,
        int fromDistrictId = 1448 //Quận 6

    );
}
