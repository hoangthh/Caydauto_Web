using Bogus;

public static class SeedData
{
    public static List<Category> GetCategories(int count)
    {
        int id = 1;
        var faker = new Faker<Category>()
            .RuleFor(c => c.Id, f => id++)
            .RuleFor(c => c.Name, f => f.Commerce.Department())
            .RuleFor(c => c.Description, f => f.Lorem.Sentence(5, 10));

        return faker.Generate(count);
    }

    public static (
        List<Product> Products,
        List<(int ProductId, int CategoryId)> Relationships
    ) GetProducts(int count, List<Category> categories)
    {
        int id = 1;
        var random = new Random();
        var faker = new Faker<Product>()
            .RuleFor(p => p.Id, f => id++)
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Brand, f => f.Company.CompanyName())
            .RuleFor(p => p.StockQuantity, f => f.Random.Int(0, 100))
            .RuleFor(p => p.Sold, f => f.Random.Int(0, 50))
            .RuleFor(p => p.CreatedDate, f => f.Date.Past(1))
            .RuleFor(p => p.UpdatedDate, f => f.Date.Recent(30));

        var products = faker.Generate(count);
        var relationships = new List<(int ProductId, int CategoryId)>();

        // Gán ngẫu nhiên 1-3 danh mục cho mỗi sản phẩm và lưu mối quan hệ
        foreach (var product in products)
        {
            var assignedCategories = categories
                .OrderBy(c => random.Next())
                .Take(random.Next(1, Math.Min(4, categories.Count)))
                .ToList();

            product.Categories = assignedCategories;

            // Lưu mối quan hệ vào danh sách
            foreach (var category in assignedCategories)
            {
                relationships.Add((product.Id, category.Id));
            }
        }

        return (products, relationships);
    }
}
