using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<User, Role, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public override int SaveChanges()
    {
        UpdateDateTracking();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateDateTracking();
        return base.SaveChangesAsync(cancellationToken);
    }

    //Phương thức này dùng để cập nhật thời gian tạo và sửa đổi cho các đối tượng có thể theo dõi thời gian
    private void UpdateDateTracking()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e =>
                e.Entity is IDateTracking
                && (e.State == EntityState.Added || e.State == EntityState.Modified)
            );

        foreach (var entry in entries)
        {
            var dateTrackingEntity = (IDateTracking)entry.Entity;
            // Sử dụng DateTimeOffset cho thời gian với múi giờ Việt Nam
            var vietnamTime = DateTime.Now.AddHours(7); // UTC+7

            if (entry.State == EntityState.Added)
            {
                dateTrackingEntity.CreatedDate = vietnamTime; // Cập nhật thời gian tạo
            }

            dateTrackingEntity.UpdatedDate = vietnamTime; // Cập nhật thời gian sửa đổi
        }
    }

    // DbSets
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<CommentImage> CommentImages { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Discount> Discounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        int ProductCount = 10; // Số lượng sản phẩm mẫu
        // Tạo dữ liệu mẫu
        var categories = SeedData.GetCategories();
        var colors = SeedData.GetColors(); // Lấy danh sách màu sắc
        var images = SeedData.GetImages(ProductCount); // Lấy danh sách hình ảnh
        var (products, relationshipProductCategory, relationshipProductColor, relationshipProductImage) = // Lấy danh sách sản phẩm và các mối
            SeedData.GetProducts(ProductCount, categories, colors, images); // 10 sản phẩm và mối quan hệ

        // Seed Categories (clear navigation property)
        foreach (var category in categories)
        {
            category.Products = null;
        }
        // Seed Colors (clear navigation property)
        foreach (var color in colors)
        {
            color.Products = null;
        }
       
        // Seed Images (clear navigation property)

        modelBuilder.Entity<Color>().HasData(colors);
        modelBuilder.Entity<Category>().HasData(categories);
        // Seed Products (clear navigation property)
        foreach (var product in products)
        {
            product.Categories = null;
        }
        foreach (var product in products)
        {
            product.Colors = null;
        }
        foreach (var product in products)
        {
            product.Images = null; // Xóa navigation tránh lỗi
        }

        modelBuilder.Entity<Product>().HasData(products);
        modelBuilder.Entity<Image>().HasData(images);
        int imagesPerProduct = 10;
        int productIdinit = 1;
        int imageCount = 0;
        foreach (var image in images)
        {
            image.Product = null; // Xóa navigation property
            // Chỉ gán ProductId nếu chưa vượt quá số sản phẩm có sẵn
            if (productIdinit <= ProductCount)
            {
                image.ProductId = productIdinit;
            }
            else
            {
                // Nếu đã vượt quá, gán cho sản phẩm cuối cùng (hoặc xử lý theo logic mong muốn)
                image.ProductId = ProductCount;
            }

            imageCount++;
            if (imageCount == imagesPerProduct)
            {
                imageCount = 0;
                productIdinit++;
            }
        }
        // Seed the many-to-many relationship (CategoryProduct)
        var productCategoryData = relationshipProductCategory
            .Select(r => new { ProductsId = r.Item1, CategoriesId = r.Item2 })
            .ToList();
        var productColorData = relationshipProductColor
            .Select(r => new { ProductsId = r.Item1, ColorsId = r.Item2 })
            .ToList();
        var productImageData = relationshipProductImage
            .Select(r => new { ProductsId = r.Item1, ImagesId = r.Item2 })
            .ToList();
        // Many-to-many: Product - Category
        modelBuilder
            .Entity<Product>()
            .HasMany(p => p.Categories)
            .WithMany(c => c.Products)
            .UsingEntity(j => j.HasData(productCategoryData));

        // Many-to-many: Product - Color
        modelBuilder
            .Entity<Product>()
            .HasMany(navigationExpression: p => p.Colors)
            .WithMany(c => c.Products)
            .UsingEntity(j => j.HasData(productColorData));

        // Many-to-many: User - Product (WishList)
        modelBuilder.Entity<User>().HasMany(u => u.WishList).WithMany(p => p.WishListedByUsers);

        // One-to-one: User - Cart
        modelBuilder
            .Entity<User>()
            .HasOne(u => u.Cart)
            .WithOne(c => c.User)
            .HasForeignKey<Cart>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Khi xóa User, xóa luôn Cart

        // One-to-many: Product - Image
        modelBuilder
            .Entity<Product>()
            .HasMany(p => p.Images)
            .WithOne(i => i.Product)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            ; // Khi xóa Product, xóa luôn Images

        // One-to-many: Product - Comment
        modelBuilder
            .Entity<Product>()
            .HasMany(p => p.Comments)
            .WithOne(c => c.Product)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.Cascade); // Khi xóa Product, xóa luôn Comments

        // One-to-many: User - Comment
        modelBuilder
            .Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Xóa Comment khi xóa User

        // One-to-many: Comment - CommentImage
        modelBuilder
            .Entity<Comment>()
            .HasMany(c => c.Images)
            .WithOne(ci => ci.Comment)
            .HasForeignKey(ci => ci.CommentId)
            .OnDelete(DeleteBehavior.Cascade); // Khi xóa Comment, xóa luôn CommentImages

        // One-to-many: User - Order
        modelBuilder
            .Entity<User>()
            .HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.SetNull); // Khi xóa User, UserId trong Order thành null

        // One-to-many: Order - OrderItem
        modelBuilder
            .Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // Khi xóa Order, xóa luôn OrderItems

        // One-to-many: Cart - CartItem
        modelBuilder
            .Entity<Cart>()
            .HasMany(c => c.CartItems)
            .WithOne(ci => ci.Cart)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade); // Khi xóa Cart, xóa luôn CartItems

        modelBuilder
            .Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict); // Khi xóa Role, không cho xóa User

        modelBuilder
            .Entity<IdentityUserRole<int>>()
            .HasOne<IdentityUser<int>>()
            .WithMany()
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.NoAction); // ⚡ Đổi từ Restrict -> NoAction

        modelBuilder
            .Entity<IdentityUserRole<int>>()
            .HasOne<IdentityRole<int>>()
            .WithMany()
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.NoAction); // ⚡ Đổi từ Restrict -> NoAction
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles"); // Đặt tên cho bảng UserRoles
        modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles"); // Đặt tên cho bảng Roles
        modelBuilder.Entity<IdentityUser<int>>().ToTable("Users"); // Đặt tên cho bảng Users
        modelBuilder
            .Entity<Role>()
            .HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "Administrator role",
                }
            );
        modelBuilder
            .Entity<Role>()
            .HasData(
                new Role
                {
                    Id = 2,
                    Name = "User",
                    NormalizedName = "USER",
                    Description = "User role",
                }
            );
        // Index cho các trường thường xuyên được tìm kiếm
        modelBuilder.Entity<Product>().HasIndex(p => p.Name).IsUnique(false); // Index cho tên sản phẩm

        modelBuilder.Entity<Order>().HasIndex(o => o.OrderDate); // Index cho ngày đặt hàng

        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique(); // Đảm bảo email là unique
    }
}
