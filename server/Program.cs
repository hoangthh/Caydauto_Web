using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Thêm DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Thêm Identity với User và Role tùy chỉnh
builder
    .Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Thêm các dịch vụ khác
builder.Services.AddControllersWithViews();

// Thêm Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "My API",
            Version = "v1",
            Description = "API for managing products, users, and more",
            Contact = new OpenApiContact { Name = "Mach Gia Huy", Email = "huymachgia555@gmail.com" },
        }
    );
});

var app = builder.Build();
// Seed Roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    var roles = new[] { "User", "Admin" };

    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new Role { Name = roleName });
        }
    }
}

// Sử dụng Swagger và Swagger UI trong pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Đặt Swagger UI tại gốc (http://localhost:<port>/)
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
