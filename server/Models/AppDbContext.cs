using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<User, Role, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

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
        base.OnModelCreating(modelBuilder); // Quan trọng khi dùng Identity

        // Many-to-many: Product - Category
        modelBuilder.Entity<Product>().HasMany(p => p.Categories).WithMany(c => c.Products);

        // Many-to-many: Product - Color
        modelBuilder.Entity<Product>().HasMany(p => p.Colors).WithMany(c => c.Products);

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
            .OnDelete(DeleteBehavior.Cascade); // Khi xóa Product, xóa luôn Images

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
            .OnDelete(DeleteBehavior.SetNull); // Khi xóa User, UserId trong Comment thành null

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

        // Index cho các trường thường xuyên được tìm kiếm
        modelBuilder.Entity<Product>().HasIndex(p => p.Name).IsUnique(false); // Index cho tên sản phẩm

        modelBuilder.Entity<Order>().HasIndex(o => o.OrderDate); // Index cho ngày đặt hàng

        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique(); // Đảm bảo email là unique
    }
}
