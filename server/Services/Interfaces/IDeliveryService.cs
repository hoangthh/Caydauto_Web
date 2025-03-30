public interface IDeliveryService
{
    Task<List<Province>?> GetProvincesAsync();
    Task<List<District>?> GetDistrictsAsync(int provinceId);
    Task<List<Ward>?> GetWardsAsync(int districtId);
    Task<int> GetShippingFeeAsync(
        int toDistrictId,
        string toWardCode,
        int insuranceValue,
        int fromDistrictId = 1448
    );
}
