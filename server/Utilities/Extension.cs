using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

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

public static class StringExtensions
{
    public static string ToNormalizedLower(this string input)
    {
        if (input == null)
            return string.Empty;

        string normalized = input.Normalize(NormalizationForm.FormD);
        var chars = normalized
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .ToArray();
        return new string(chars).Normalize(NormalizationForm.FormC).ToLower();
    }

    public static string RemoveDiacritics(this string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var chars = normalized.Where(c =>
            CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark
        );
        return new string(chars.ToArray()).Normalize(NormalizationForm.FormC);
    }
}


public static class ExpressionHelper
{
    // Hàm build so sánh, xài chung cho cả Property vs Value và Property vs Property
    private static Expression BuildComparisonExpression(
        Expression left,
        Expression right,
        string comparison
    )
    {
        return comparison switch
        {
            "Equals" => Expression.Equal(left, right),
            "GreaterThan" => Expression.GreaterThan(left, right),
            "LessThan" => Expression.LessThan(left, right),
            "Contains" => Expression.Call(left, "Contains", Type.EmptyTypes, right),
            "StartsWith" => Expression.Call(left, "StartsWith", Type.EmptyTypes, right),
            "EndsWith" => Expression.Call(left, "EndsWith", Type.EmptyTypes, right),
            _ => throw new NotImplementedException($"Operator '{comparison}' chưa hỗ trợ."),
        };
    }
    // Hàm FilterByProperties
    /// <summary>
    /// Lọc dữ liệu theo các thuộc tính của đối tượng
    /// Code mẫu:
    /// var filters = new[]
    /// {
    ///     ("Name", "John", null, "Equals", null),
    ///     ("Age", 30, null, "GreaterThan", LogicOperator.And),
    ///     ("Salary", null, "MinSalary", "GreaterThan", LogicOperator.Or)
    /// };
    /// var result = source.FilterByProperties(filters);    
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của đối tượng</typeparam>
    /// <param name="source">Dữ liệu nguồn</param>
    /// <param name="filters">Danh sách các filter</param>
    /// <returns>Dữ liệu đã được lọc</returns>
    /// <exception cref="ArgumentException">Ném ra khi thiếu PropertyName hoặc Comparison</exception>
    /// <exception cref="ArgumentException">Ném ra khi Property không tồn tại trong đối tượng</exception>
    /// <exception cref="ArgumentException">Ném ra khi filter thiếu giá trị so sánh (Value hoặc OtherPropertyName)</exception>
    /// <exception cref="ArgumentException">Ném ra khi từ filter thứ hai trở đi phải có LogicOperator (AND/OR)</exception>
    /// <exception cref="NotImplementedException">Ném ra khi LogicOperator chưa được hỗ trợ</exception>
    /// <exception cref="NotImplementedException">Ném ra khi Operator chưa được hỗ trợ</exception>
    /// <example>
    /// </example>
    public static IQueryable<T> FilterByProperties<T>(
        this IQueryable<T> source,
        params (
            string PropertyName,
            object? Value,
            string? OtherPropertyName,
            string Comparison,
            LogicOperator? Logic
        )[] filters
    )
    {
        if (filters == null || filters.Length == 0)
            return source;

        Type entityType = typeof(T);
        ParameterExpression parameter = Expression.Parameter(entityType, "x");

        Expression? combinedExpression = null;

        for (int i = 0; i < filters.Length; i++)
        {
            var (propertyName, value, otherPropertyName, comparison, logic) = filters[i];

            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(comparison))
            {
                throw new ArgumentException("Thiếu PropertyName hoặc Comparison.");
            }

            PropertyInfo property =
                entityType.GetProperty(propertyName)
                ?? throw new ArgumentException(
                    $"Property '{propertyName}' không tồn tại trong '{typeof(T).Name}'."
                );

            Expression left = Expression.Property(parameter, property);

            Expression right;
            if (!string.IsNullOrEmpty(otherPropertyName))
            {
                PropertyInfo otherProperty =
                    entityType.GetProperty(otherPropertyName)
                    ?? throw new ArgumentException(
                        $"Property '{otherPropertyName}' không tồn tại trong '{typeof(T).Name}'."
                    );

                right = Expression.Property(parameter, otherProperty);
            }
            else if (value != null)
            {
                right = Expression.Constant(Convert.ChangeType(value, property.PropertyType));
            }
            else
            {
                throw new ArgumentException(
                    $"Filter cho '{propertyName}' thiếu giá trị so sánh (Value hoặc OtherPropertyName)."
                );
            }

            Expression comparisonExpression = BuildComparisonExpression(left, right, comparison);

            if (i == 0)
            {
                combinedExpression = comparisonExpression; // Filter đầu tiên
            }
            else
            {
                if (!logic.HasValue)
                {
                    throw new ArgumentException(
                        "Từ filter thứ hai trở đi phải có LogicOperator (AND/OR)."
                    );
                }

                combinedExpression = logic.Value.Combine(combinedExpression!, comparisonExpression);
            }
        }

        var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression!, parameter);
        return source.Where(lambda);
    }
}

public static class LogicOperatorExtensions
{
    public static Expression Combine(
        this LogicOperator logicOperator,
        Expression left,
        Expression right
    )
    {
        return logicOperator switch
        {
            LogicOperator.And => Expression.AndAlso(left, right),
            LogicOperator.Or => Expression.OrElse(left, right),
            _ => throw new NotImplementedException(
                $"LogicOperator '{logicOperator}' chưa được hỗ trợ."
            ),
        };
    }
}
