using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scrutor;
using server.Services.Mapping;

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

    public static IQueryable<T> OrderByMultiple<T>(
        this IQueryable<T> source,
        params (string property, bool descending)[] orders
    )
    {
        Type entityType = typeof(T);
        ParameterExpression parameter = Expression.Parameter(entityType, "x");

        IQueryable<T> result = source;

        for (int i = 0; i < orders.Length; i++)
        {
            (string property, bool descending) order = orders[i];

            PropertyInfo? propertyInfo = entityType.GetProperty(order.property);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{order.property}' không tồn tại.");
            }

            MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);
            LambdaExpression orderByExp = Expression.Lambda(propertyAccess, parameter);

            string methodName =
                i == 0
                    ? (order.descending ? "OrderByDescending" : "OrderBy")
                    : (order.descending ? "ThenByDescending" : "ThenBy");

            MethodCallExpression orderCall = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { entityType, propertyInfo.PropertyType },
                result.Expression,
                Expression.Quote(orderByExp)
            );

            result = result.Provider.CreateQuery<T>(orderCall);
        }

        return result;
    }

    public static IQueryable<T> IncludeMultiple<T>(
        this IQueryable<T> query,
        params Expression<Func<T, object>>[] includes
    )
        where T : class
    {
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query;
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

public static class ServiceExtensions
{
    static readonly HashSet<Type> excludedTypes = new HashSet<Type> { typeof(UserIdMiddleware) };

    public static void AddProjectServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        ConfigureCors(services);
        services.AddControllers();
        ConfigureAutoMapper(services);
        ConfigureAuthentication(services);
        ConfigureRedis(services);
        ConfigureMemoryCache(services);
        ConfigureSwagger(services);
        services.AddHttpClient(); // Thêm HttpClient vào DI container
        services.Scan(scan =>
            scan.FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(classes => classes.Where(type => !excludedTypes.Contains(type))) // Sử dụng Where() thay vì toán tử !
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .AsSelf()
                .WithScopedLifetime()
        );

        ConfigureScopedServices(services);
        ConfigureSessionService(services);
        ConfigureEntityFramework(services, configuration);
    }

    private static void ConfigureAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
    }

    private static void ConfigureRedis(IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"localhost:{Constraint.Port.Redis}"; // thay đổi cấu hình theo môi trường của bạn
            options.InstanceName = "CayDauToInstance";
        });
    }

    /// <summary>
    /// Cấu hình dịch vụ Session
    /// </summary>
    private static void ConfigureSessionService(IServiceCollection services)
    {
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromDays(1); // Set session timeout
            options.Cookie.HttpOnly = true; // Make the session cookie HTTP only
            options.Cookie.IsEssential = true; // Make the session cookie essential
        });
    }

    /// <summary>
    /// Cấu hình EntityFramework.
    /// </summary>
    private static void ConfigureEntityFramework(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services
            .AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }

    private static void ConfigureSwagger(IServiceCollection services)
    {
        // Thêm Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1",
                    Description = "API for managing products, users, and more",
                    Contact = new OpenApiContact
                    {
                        Name = "Mach Gia Huy",
                        Email = "huymachgia555@gmail.com",
                    },
                }
            );
        });
    }

    /// <summary>
    /// Cấu hình dịch vụ xác thực bằng cookie.
    /// </summary>
    private static void ConfigureAuthentication(IServiceCollection services)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Đặt DefaultSignInScheme
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "ShUEHApplication-Cookies-Authentication"; // Tên của cookie
                options.Cookie.HttpOnly = true; // Cookie chỉ có thể truy cập qua HTTP
                options.Cookie.IsEssential = true; // Đảm bảo cookie được gửi ngay cả khi người dùng chưa đồng ý
                options.Cookie.SameSite = SameSiteMode.Unspecified;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddGoogle(options =>
            {
                options.ClientId =
                    "11161045560-r08im8g3rll7ifg200tgmc8gmpa1am1t.apps.googleusercontent.com"; // Thay bằng Client ID của bạn
                options.ClientSecret = "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"; // Thay bằng Client Secret của bạn
                options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                options.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
            });
    }

    private static void ConfigureSingletonServices(IServiceCollection services) { }

    /// <summary>
    /// Cấu hình các dịch vụ Transient.
    /// </summary>
    private static void ConfigureTransientServices(IServiceCollection services)
    {
        // Thêm các dịch vụ transient khác nếu cần
    }

    /// <summary>
    /// Cấu hình các dịch vụ Cache
    /// </summary>
    private static void ConfigureMemoryCache(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddDistributedMemoryCache();
    }

    /// <summary>
    /// Cấu hình các dịch vụ Scoped.
    /// </summary>
    private static void ConfigureScopedServices(IServiceCollection services)
    {
        // /*Repository*/
        // services.AddScoped<IProductRepository, ProductRepository>();
        // services.AddScoped<ICategoryRepository, CategoryRepository>();
        // services.AddScoped<ICartRepository, CartRepository>();
        // services.AddScoped<IRoleRepository, RoleRepository>();
        // services.AddScoped<IUserRepostory, UserRepository>();
        // /*Services*/
        // services.AddScoped<IProductService, ProductService>();
        // services.AddScoped<ICurrentUserService, CurrentUserService>();
        // services.AddScoped<ICartService, CartService>();
        // services.AddScoped<IEmailSender, EmailService>();
        // services.AddScoped<IAccountService, AccountService>();
        // services.AddScoped<IDeliveryService, DeliveryService>();

        //services.AddScoped<FileService>();
        /*Others*/
        //services.AddScoped<HttpClient>();
        /*Identity*/
        //services.AddScoped<UserManager<User>>(); // Quản lý người dùng trong hệ thống
        //services.AddScoped<SignInManager<User>>(); // Quản lý đăng nhập người dùng
    }

    /// <summary>
    /// Cấu hình dịch vụ CORS.
    /// </summary>
    private static void ConfigureCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(
                name: "CorsPolicy",
                builder =>
                {
                    builder
                        .WithOrigins(Constraint.Url.Server, Constraint.Url.Client)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    // Allow localhost origins
                    // builder.WithOrigins("http://localhost:3000", "http://localhost:5118") // Liệt kê các origin cụ thể
                    //     .AllowAnyHeader()
                    //     .AllowAnyMethod()
                    //     .AllowCredentials();
                }
            );
        });
    }
}
