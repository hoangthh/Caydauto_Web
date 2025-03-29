using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProjectServices(builder.Configuration);
var app = builder.Build();

// Seed Roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    var roles = new[] { "User", "Admin" };

    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName).ConfigureAwait(false))
        {
            await roleManager.CreateAsync(new Role { Name = roleName }).ConfigureAwait(false);
        }
    }
}
// Sử dụng Swagger và Swagger UI trong pipeline

// Middleware Pipeline
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseSession(); // Before authentication
app.UseAuthentication(); // Must come before custom middleware and authorization
app.UseUserIdMiddleware(); // Assuming this depends on authenticated user
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Swagger UI at root
});
app.MapControllers();

app.Run();
