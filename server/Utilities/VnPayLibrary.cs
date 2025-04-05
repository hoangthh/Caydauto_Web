using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

public class VnPayLibrary
{
    // Danh sách sắp xếp để lưu trữ dữ liệu yêu cầu và phản hồi
    private readonly SortedList<string, string> _requestData = new SortedList<string, string>(
        new VnPayCompare()
    );
    private readonly SortedList<string, string> _responseData = new SortedList<string, string>(
        new VnPayCompare()
    );

    // Phương thức thêm dữ liệu yêu cầu
    public void AddRequestData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(key))
        {
            _requestData.Add(key, value);
        }
    }

    // Phương thức thêm dữ liệu phản hồi
    public void AddResponseData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _responseData.Add(key, value);
        }
    }

    // Phương thức lấy dữ liệu phản hồi theo key
    public string GetResponseData(string key)
    {
        return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
    }

    #region Request
    // Phương thức tạo URL yêu cầu thanh toán
    public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
    {
        var data = new StringBuilder();

        // Duyệt qua các cặp key-value trong _requestData
        foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
        {
            data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
        }

        var querystring = data.ToString();

        // Thêm querystring vào baseUrl
        baseUrl += "?" + querystring;
        var signData = querystring;
        if (signData.Length > 0)
        {
            signData = signData.Remove(data.Length - 1, 1);
        }

        // Tạo chữ ký bảo mật
        var vnpSecureHash = Utils.HmacSHA512(vnpHashSecret, signData);
        baseUrl += "vnp_SecureHash=" + vnpSecureHash;

        return baseUrl;
    }
    #endregion

    #region Response process
    // Phương thức xác thực chữ ký bảo mật
    public bool ValidateSignature(string inputHash, string secretKey)
    {
        var rspRaw = GetResponseData();
        var myChecksum = Utils.HmacSHA512(secretKey, rspRaw);
        return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
    }

    // Phương thức lấy dữ liệu phản hồi dưới dạng chuỗi
    private string GetResponseData()
    {
        var data = new StringBuilder();
        if (_responseData.ContainsKey("vnp_SecureHashType"))
        {
            _responseData.Remove("vnp_SecureHashType");
        }

        if (_responseData.ContainsKey("vnp_SecureHash"))
        {
            _responseData.Remove("vnp_SecureHash");
        }

        // Duyệt qua các cặp key-value trong _responseData
        foreach (var (key, value) in _responseData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
        {
            data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
        }

        // Xóa ký tự '&' cuối cùng
        if (data.Length > 0)
        {
            data.Remove(data.Length - 1, 1);
        }

        return data.ToString();
    }
    #endregion
}

public class Utils
{
    // Phương thức tạo HMAC SHA512
    public static string HmacSHA512(string key, string inputData)
    {
        var hash = new StringBuilder();
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var inputBytes = Encoding.UTF8.GetBytes(inputData);
        using (var hmac = new HMACSHA512(keyBytes))
        {
            var hashValue = hmac.ComputeHash(inputBytes);
            foreach (var theByte in hashValue)
            {
                hash.Append(theByte.ToString("x2"));
            }
        }

        return hash.ToString();
    }

    // Phương thức lấy địa chỉ IP
    public static string GetIpAddress(HttpContext context)
    {
        var ipAddress = string.Empty;
        try
        {
            var remoteIpAddress = context.Connection.RemoteIpAddress;

            if (remoteIpAddress != null)
            {
                if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = Dns.GetHostEntry(remoteIpAddress)
                        .AddressList.FirstOrDefault(x =>
                            x.AddressFamily == AddressFamily.InterNetwork
                        );
                }

                if (remoteIpAddress != null)
                    ipAddress = remoteIpAddress.ToString();

                return ipAddress;
            }
        }
        catch (Exception ex)
        {
            return "Invalid IP:" + ex.Message;
        }

        return "127.0.0.1";
    }
}

public class VnPayCompare : IComparer<string>
{
    // Phương thức so sánh chuỗi
    public int Compare(string? x, string? y)
    {
        if (x == y)
            return 0;
        if (x == null)
            return -1;
        if (y == null)
            return 1;
        var vnpCompare = CompareInfo.GetCompareInfo("en-US");
        return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
    }
}
