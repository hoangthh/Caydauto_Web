using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IAccountService accountService,
        ILogger<AccountController> logger,
        ICurrentUserService currentUserService
    )
    {
        _logger = logger;
        _accountService = accountService;
        _currentUserService = currentUserService;
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var user = await _accountService.GetUserProfileAsync().ConfigureAwait(false);
        if (user == null)
            return NotFound("User not found");
        return Ok(user);
    }

    [HttpGet("profile/{id}")]
    public async Task<IActionResult> GetProfileById(int id)
    {
        var user = await _accountService.GetUserByIdAsync(id).ConfigureAwait(false);
        if (user == null)
            return NotFound("User not found");
        return Ok(user);
    }

    [HttpGet("state")]
    public async Task<IActionResult> GetState()
    {
        if (_currentUserService.UserId != null)
        {
            return Ok(await _currentUserService.GetUser().ConfigureAwait(false));
        }
        return Unauthorized("User had not logged in");
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO model)
    {
        var result = await _accountService.RegisterAsync(model).ConfigureAwait(false);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _accountService.LoginAsync(model).ConfigureAwait(false);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> LogoutAsync()
    {
        await _accountService.LogoutAsync().ConfigureAwait(false);
        return Ok(new { Message = "Logout successful" });
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync(string token, string email)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            return BadRequest("Invalid token or email");
        var result = await _accountService.VerifyEmailAsync(token, email).ConfigureAwait(false);
        if (result)
            return Ok(new { Message = "Email confirmed successfully" });
        return BadRequest(new { Message = "Email confirmation failed" });
    }

    [HttpGet("google-login")]
    public IActionResult SignInGoogle()
    {
        var rememberMe = Request.Query["rememberMe"] == "true";
        var redirectUrl = $"{Constraint.Url.Server}/api/account/google-authen"; // Đường dẫn chuyển hướng sau khi xác thực thành công
        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUrl,
            IsPersistent = rememberMe, // Lưu thông tin đăng nhập nếu rememberMe là true};
        };
        _logger.LogInformation($"Redirecting to Google for authentication, {redirectUrl} ");
        return Challenge(properties, GoogleDefaults.AuthenticationScheme); // Chuyển hướng tới Google
    }

    [HttpGet("google-authen")]
    public async Task<IActionResult> GoogleAuthenAsync()
    {
        _logger.LogInformation("Google authentication started.");
        // Lấy thông tin xác thực từ Google
        var info = await HttpContext
            .AuthenticateAsync(GoogleDefaults.AuthenticationScheme)
            .ConfigureAwait(false);
        if (!info.Succeeded)
            return Redirect(Constraint.Url.Client); // Chuyển hướng về trang đăng nhập nếu không thành công
        var result = await _accountService.GoogleAuthen(info).ConfigureAwait(false); // Gọi phương thức GoogleAuthen trong AccountService\
        if (result.IsSuccess)
            return Redirect(result.RedirectUrl); // Chuyển hướng về trang chủ nếu thành công
        return Redirect($"{Constraint.Url.Client}/user/login"); // Chuyển hướng về trang đăng nhập nếu không thành công
    }
}
