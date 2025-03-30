using Bogus;
using Microsoft.AspNetCore.Identity;

public static class SeedData
{
    // Danh sách danh mục có sẵn
    private static int ImagePerProduct = 10;
    private static readonly List<string> Categories = new()
    {
        "Hộp bút - Túi bút",
        "Vở Học Sinh Campus",
        "Bút gel",
        "Bút chì",
        "Bút ghi nhớ",
        "Dụng cụ tẩy",
        "Bút brush",
        "Băng keo",
        "Dụng cụ hỗ trợ",
    };

    // Danh sách thương hiệu có sẵn
    private static readonly List<string> Brands = new()
    {
        "Crayola",
        "Pilot",
        "Kokuyo",
        "Marunman",
        "Marvy",
        "Zebra",
        "Stabilo",
        "Tombow",
        "Sakura",
        "Plus",
    };

    private static readonly List<string> Colors = new()
    {
        "Black",
        "White",
        "Gray",
        "Orange",
        "Yellow",
        "Green",
        "Blue",
        "Purple",
        "Pink",
        "Red",
        "Brown",
    };

    public static readonly Dictionary<string, (decimal Min, decimal Max)> PriceRanges = new()
    {
        { "Dưới 100.000", (10, 100) },
        { "100.000 - 200.000", (100, 200) },
        { "200.000 - 300.000", (200, 300) },
        { "300.000 - 400.000", (300, 400) },
        { "400.000 - 500.000", (400, 500) },
        { "Trên 500.000", (500, 1000) },
    };

    public static List<Image> GetImages(int productCount)
    {
        int id = 1;
        var faker = new Faker<Image>()
            .RuleFor(i => i.Id, f => id++)
            .RuleFor(i => i.Url, f => f.Image.PicsumUrl(width: 500, height: 500));
        return faker.Generate(ImagePerProduct * productCount); // Giả sử mỗi sản phẩm có 10 hình ảnh
    }

    public static List<Category> GetCategories()
    {
        int id = 1;
        return Categories
            .Select(name => new Category
            {
                Id = id++,
                Name = name,
                Description = $"Danh mục {name} chứa các sản phẩm liên quan.",
            })
            .ToList();
    }

    public static List<Color> GetColors()
    {
        int id = 1;
        return Colors.Select(name => new Color { Id = id++, Name = name }).ToList();
    }

    public static (List<Product>, List<(int, int)>, List<(int, int)>, List<(int, int)>) GetProducts(
        int count,
        List<Category> categories,
        List<Color> colors,
        List<Image> images
    )
    {
        int id = 1;
        var random = new Random();
        var faker = new Faker<Product>()
            .RuleFor(p => p.Id, f => id++)
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Brand, f => f.PickRandom(Brands)) // Chọn brand riêng biệt
            .RuleFor(
                p => p.Price,
                f =>
                {
                    var range = f.PickRandom(PriceRanges.Values.ToList()); // Chọn ngẫu nhiên một khoảng giá từ PriceRanges
                    return f.Random.Decimal(range.Min, range.Max); // Tạo giá ngẫu nhiên trong khoảng đã chọn
                    // Chọn ngẫu nhiên một khoảng giá từ PriceRanges
                }
            )
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.StockQuantity, f => f.Random.Int(0, 100))
            .RuleFor(p => p.Sold, f => f.Random.Int(0, 50))
            .RuleFor(p => p.CreatedDate, f => f.Date.Past(1))
            .RuleFor(p => p.UpdatedDate, f => f.Date.Recent(30));

        var products = faker.Generate(count);
        var relationshipsProductCategory = new List<(int ProductId, int CategoryId)>();
        var relationshipsProductColor = new List<(int ProductId, int ColorId)>();
        var relationshipsProductImage = new List<(int ProductId, int ImageId)>();
        int countImage = ImagePerProduct;
        var assignImages = images.Take(countImage).ToList(); // Giả sử mỗi sản phẩm có 10 hình ảnh
        // Gán danh mục cho từng sản phẩm
        foreach (var product in products)
        {
            var assignedCategories = categories
                .OrderBy(c => random.Next())
                .Take(random.Next(1, Math.Min(4, categories.Count)))
                .ToList();
            var assignColors = colors
                .OrderBy(c => random.Next())
                .Take(random.Next(1, Math.Min(4, colors.Count)))
                .ToList();

            product.Categories = assignedCategories;
            product.Colors = assignColors;
            product.Images = assignImages;
            // Gán danh mục cho sản phẩm
            // Lưu mối quan hệ vào danh sách
            foreach (var category in assignedCategories)
            {
                relationshipsProductCategory.Add((product.Id, category.Id));
            }
            foreach (var color in assignColors)
            {
                relationshipsProductColor.Add((product.Id, color.Id));
            }
            foreach (var image in assignImages)
            {
                relationshipsProductImage.Add((product.Id, image.Id));
            }
            countImage += ImagePerProduct;
        }

        return (
            products,
            relationshipsProductCategory,
            relationshipsProductColor,
            relationshipsProductImage
        );
    }
}

public static class IdentityInitializer
{
    public static async Task InitAdminUser(IServiceProvider serviceProvider)
    {
        // Lấy UserManager và SignInManager từ service provider
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var signInManager = serviceProvider.GetRequiredService<SignInManager<User>>();

        // Thông tin tài khoản admin
        string adminEmail = "admin@example.com";
        string adminPassword = "Admin@123";
        string adminUserName = "admin";

        // Kiểm tra xem tài khoản admin đã tồn tại chưa
        var adminUser = await userManager.FindByEmailAsync(adminEmail).ConfigureAwait(false);
        if (adminUser == null)
        {
            // Tạo tài khoản admin
            adminUser = new User
            {
                FullName = "Mạch Gia Huy",
                Gender = "Male",
                UserName = adminUserName,
                Email = adminEmail,
                EmailConfirmed = true, // Bỏ qua xác nhận email
                RoleId = 1,
            };

            var result = await userManager
                .CreateAsync(adminUser, adminPassword)
                .ConfigureAwait(false);
            if (result.Succeeded)
            {
                // Gán vai trò Admin (nếu bạn có vai trò)
                await userManager.AddToRoleAsync(adminUser, "Admin").ConfigureAwait(false);
                Console.WriteLine("Tài khoản admin đã được tạo thành công!");
            }
            else
            {
                throw new Exception(
                    "Không thể tạo tài khoản admin: "
                        + string.Join(", ", result.Errors.Select(e => e.Description))
                );
            }
        }
    }
}
