

public interface IAddressService
{
    Task<(bool Success, string Message)> GetAddress(
        string shippingAddress,
        int provinceId,
        int districtId,
        string WardCode
    );
}
