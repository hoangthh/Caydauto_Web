using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class DeliveryService : IDeliveryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DeliveryService> _logger;
    private readonly string _token;
    private readonly int _shopId = 5704602;

    public DeliveryService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<DeliveryService> logger
    )
    {
        _logger = logger;
        _httpClient = httpClient;
        _token = configuration["DeliveryAPI:SecretKey"] ?? "null";
        _logger.LogInformation(
            $"DeliveryService initialized with token {_token} and shopId {_shopId}"
        );

        // Đặt BaseAddress cho toàn bộ API GHN
        _httpClient.BaseAddress = new Uri("https://online-gateway.ghn.vn/shiip/public-api/");
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json")
        );
        _httpClient.DefaultRequestHeaders.Add("token", _token);
        _httpClient.DefaultRequestHeaders.Add("ShopId", _shopId.ToString()); // Thêm Shop-Id vào header
    }

    public async Task<string> GetProvincesAsync()
    {
        var response = await _httpClient.GetAsync("master-data/province");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetDistrictsAsync(int provinceId)
    {
        var response = await _httpClient.GetAsync($"master-data/district?province_id={provinceId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetWardsAsync(int districtId)
    {
        var response = await _httpClient.GetAsync($"master-data/ward?district_id={districtId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetAvailableServicesAsync(int fromDistrictId, int toDistrictId)
    {
        var requestBody = new
        {
            from_district = fromDistrictId,
            to_district = toDistrictId,
            shop_id = _shopId,
        };

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(
            "v2/shipping-order/available-services",
            jsonContent
        );

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"GHN API Error {response.StatusCode}: {responseContent}");
            throw new Exception($"API Error {response.StatusCode}: {responseContent}");
        }

        return responseContent;
    }

    public async Task<string> GetShippingFeeAsync(
        int toDistrictId,
        string toWardCode,
        int insuranceValue,
        int fromDistrictId = 1448
    )
    {
        var requestBody = new
        {
            service_id = 53320,
            insurance_value = insuranceValue,
            coupon = "",
            from_district_id = fromDistrictId,
            to_district_id = toDistrictId,
            to_ward_code = toWardCode,
            weight = 300,
            length = 10,
            width = 10,
            height = 10,
        };

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync("v2/shipping-order/fee", jsonContent);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"GHN API Error {response.StatusCode}: {responseContent}");
            return $"API Error {response.StatusCode}: {responseContent}";
        }

        return responseContent;
    }
}
