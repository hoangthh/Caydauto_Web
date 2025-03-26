using System.Globalization;

public static class DateTimeExtensions
{
    // Chuyển đổi ngày tháng thành chuỗi định dạng mong muốn
    /// <summary>
    /// Chuyển đổi ngày tháng thành chuỗi định dạng mong muốn
    /// </summary>
    public static string FormatDate(this DateTime date, string format = "dd/MM/yyyy")
    {
        return date.ToString(format);
    }
    // Chuyển đổi chuỗi thành kiểu DateTime
    /// <summary>
    /// Chuyển đổi chuỗi thành kiểu DateTime
    /// </summary>

    public static DateTime? ToDateTime(this string dateString, string format = "dd/MM/yyyy")
    {
        // Phân tách ngày, tháng, năm
        var parts = dateString.Split('/');
        if (parts.Length != 3)
        {
            throw new ArgumentException($"Chuỗi '{dateString}' không có định dạng đúng.");
        }

        // Thêm số 0 ở đầu cho ngày và tháng nếu cần
        string day = parts[0].PadLeft(2, '0');
        string month = parts[1].PadLeft(2, '0');
        string year = parts[2];

        // Tạo lại chuỗi ngày tháng năm
        string formattedDateString = $"{day}/{month}/{year}";

        DateTime parsedDate;

        // Sử dụng TryParseExact để tránh ngoại lệ nếu chuỗi không đúng định dạng
        if (
            DateTime.TryParseExact(
                formattedDateString,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out parsedDate
            )
        )
        {
            return parsedDate; // Trả về giá trị DateTime nếu chuyển đổi thành công
        }

        // Ném ra ngoại lệ nếu không thể chuyển đổi
        throw new ArgumentException(
            $"Chuỗi '{dateString}' không thể chuyển đổi thành kiểu DateTime với định dạng '{format}'."
        );
    }
}
