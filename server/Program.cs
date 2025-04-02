using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProjectServices(builder.Configuration);
var app = builder.Build();

if (args.Contains("seed"))
{
    using (var scope = app.Services.CreateScope())
    {
        await IdentityInitializer.InitAdminUser(scope.ServiceProvider).ConfigureAwait(false);
        Console.WriteLine("Data seeding completed.");
        return;
    }
}

// Sử dụng Swagger và Swagger UI trong pipeline

// Middleware Pipeline

//app.UseMiddleware<EnableRewindableBodyStartup>();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseRouting();
app.UseSession(); // Before authentication
app.UseAuthentication(); // Must come before custom middleware and authorization
app.UseAuthorization();
app.UseUserIdMiddleware(); // Assuming this depends on authenticated user
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = "swagger"; // Đặt Swagger UI tại /swagger
});
app.MapControllers();

app.Run();
