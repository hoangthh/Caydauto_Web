using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

public class DeliveryService : IDeliveryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DeliveryService> _logger;
    private readonly string _token;
    private readonly int _shopId = 5704602;
    private readonly IDistributedCache _cache;
    private readonly JsonSerializerOptions _options;

    public DeliveryService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<DeliveryService> logger,
        IDistributedCache cache
    )
    {
        _logger = logger;
        _cache = cache;
        _httpClient = httpClientFactory.CreateClient();
        _token = configuration["DeliveryAPI:SecretKey"] ?? "null";
        _logger.LogInformation(
            $"DeliveryService initialized with token {_token} and shopId {_shopId}"
        );
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            // Các trường không có trong DTO sẽ tự động bị bỏ qua
        };
        // Đặt BaseAddress cho toàn bộ API GHN
        _httpClient.BaseAddress = new Uri("https://online-gateway.ghn.vn/shiip/public-api/");
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json")
        );
        _httpClient.DefaultRequestHeaders.Add("token", _token);
        _httpClient.DefaultRequestHeaders.Add("ShopId", _shopId.ToString()); // Thêm Shop-Id vào header
    }

    public async Task<List<Province>?> GetProvincesAsync()
    {
        try
        {
            const string cacheKey = "Provinces";
            // Kiểm tra dữ liệu trong cache
            var cachedData = await _cache.GetStringAsync(cacheKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedData))
            {
                var filterData = JsonSerializer.Deserialize<List<Province>>(cachedData);
                return filterData;
            }
            // Thêm headers nếu cần

            var response = await _httpClient.GetAsync("master-data/province").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                _logger.LogError($"GHN API error: {response.StatusCode} - {errorContent}");
                return null;
            }

            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiResponse = JsonSerializer.Deserialize<GHNShippingResponse<List<Province>>>(
                responseString,
                _options
            );

            if (apiResponse?.Code != 200 || apiResponse.Data == null)
            {
                _logger.LogError($"Invalid GHN API response: {responseString}");
                return null;
            }
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30),
            };
            var provinces = JsonSerializer.Serialize(apiResponse.Data);
            await _cache.SetStringAsync(cacheKey, provinces, options).ConfigureAwait(false);
            return apiResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network error while calling GHN API");
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetProvincesAsync");
            return null;
        }
    }

    public async Task<List<District>?> GetDistrictsAsync(int provinceId)
    {
        try
        {
            var response = await _httpClient
                .GetAsync($"master-data/district?province_id={provinceId}")
                .ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                _logger.LogError($"GHN API error: {response.StatusCode} - {errorContent}");
                return null;
            }

            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiResponse = JsonSerializer.Deserialize<GHNShippingResponse<List<District>>>(
                responseString,
                _options
            );

            if (apiResponse?.Code != 200 || apiResponse.Data == null)
            {
                _logger.LogError($"Invalid GHN API response: {responseString}");
                return null;
            }
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30),
            };
            var district = JsonSerializer.Serialize(apiResponse.Data);
            return apiResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network error while calling GHN API");
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetProvincesAsync");
            return null;
        }
    }

    public async Task<List<Ward>?> GetWardsAsync(int districtId)
    {
        try
        {
            var response = await _httpClient
                .GetAsync($"master-data/ward?district_id={districtId}")
                .ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                _logger.LogError($"GHN API error: {response.StatusCode} - {errorContent}");
                return null;
            }

            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiResponse = JsonSerializer.Deserialize<GHNShippingResponse<List<Ward>>>(
                responseString,
                _options
            );

            if (apiResponse?.Code != 200 || apiResponse.Data == null)
            {
                _logger.LogError($"Invalid GHN API response: {responseString}");
                return null;
            }

            return apiResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network error while calling GHN API");
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetProvincesAsync");
            return null;
        }
    }

    public async Task<int> GetShippingFeeAsync(
        int toDistrictId,
        string toWardCode,
        int insuranceValue,
        int fromDistrictId = 1448,
        int serviceId = 53320
    )
    {
        try
        {
            var requestBody = new
            {
                service_id = serviceId,
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

            // Thêm log request
            _logger.LogInformation($"GHN Request: {JsonSerializer.Serialize(requestBody)}");

            var response = await _httpClient
                .PostAsync("v2/shipping-order/fee", jsonContent)
                .ConfigureAwait(false);

            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Thêm log response
            _logger.LogInformation($"GHN Response: {responseString}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GHN API Error: {response.StatusCode} - {responseString}");
                return -1;
            }

            var data = JsonSerializer.Deserialize<GHNShippingResponse<FeeData>>(responseString);

            if (data == null)
            {
                _logger.LogError("Failed to deserialize GHN response");
                return -1;
            }

            if (data.Code != 200)
            {
                _logger.LogError(
                    $"GHN API returned non-success code: {data.Code} - {data.Message}"
                );
                return -1;
            }

            // Sửa tại đây: Kiểm tra null và trả về Total thay vì ServiceFee
            return data.Data?.Total ?? -1;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while calling GHN API");
            return -1;
        }
    }

    public async Task<List<ServiceInfo>?> GetAvailableServicesAsync(
        int toDistrictId,
        int fromDistrictId = 1448
    )
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

        var response = await _httpClient
            .PostAsync("v2/shipping-order/available-services", jsonContent)
            .ConfigureAwait(false);
        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var apiResponse = JsonSerializer.Deserialize<GHNShippingResponse<List<ServiceInfo>>>(
            responseString,
            _options
        );

        if (apiResponse?.Code != 200 || apiResponse.Data == null)
        {
            _logger.LogError($"Invalid GHN API response: {responseString}");
            return null;
        }

        return apiResponse.Data;
    }
}

public class GHNShippingResponse<T>
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    public T Data { get; set; }
}

public class FeeData
{
    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("service_fee")]
    public int ServiceFee { get; set; }

    [JsonPropertyName("insurance_fee")]
    public int InsuranceFee { get; set; }
}

public class Province
{
    public int ProvinceID { get; set; }

    public string? ProvinceName { get; set; }
}

public class District
{
    public int DistrictID { get; set; }
    public int ProvinceID { get; set; }
    public string? DistrictName { get; set; }
}

public class Ward
{
    public string? WardCode { get; set; }
    public int DistrictID { get; set; }
    public string? WardName { get; set; }
}

public class ServiceInfo
{
    [JsonPropertyName("service_id")]
    public int ServiceId { get; set; }

    [JsonPropertyName("short_name")]
    public string ShortName { get; set; }

    [JsonPropertyName("service_type")]
    public int ServiceType { get; set; }
}
