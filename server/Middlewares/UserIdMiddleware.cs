using System.Security.Claims;

public class UserIdMiddleware
{
    private readonly RequestDelegate _next;

    public UserIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUserService)
    {
        try
        {
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (int.TryParse(userIdClaim, out int userId))
                {
                    currentUserService.SetUserId(userId);
                }
                else
                {
                    // Log warning: unable to parse user ID
                }
            }
            await _next(context);
        }
        catch
        {
            
            await _next(context); // or handle the error appropriately
        }
    }
}

// Extension method để đăng ký middleware dễ dàng
public static class UserIdMiddlewareExtensions
{
    public static IApplicationBuilder UseUserIdMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserIdMiddleware>();
    }
}
