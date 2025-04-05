using System.Security.Claims;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signinManager;
    private readonly IUserRepostory _userRepository;
    private readonly ILogger<AccountService> _logger;
    private readonly IRoleRepository _roleRepository;
    private readonly IEmailSender _emailService;
    private readonly IConfiguration _configuration;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public AccountService(
        UserManager<User> userManager,
        IEmailSender emailService,
        IConfiguration configuration,
        IUserRepostory userRepository,
        IRoleRepository roleRepository,
        SignInManager<User> signinManager,
        ILogger<AccountService> logger,
        IMapper mapper,
        ICurrentUserService currentUserService
    )
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _emailService = emailService;
        _configuration = configuration;
        _roleRepository = roleRepository;
        _signinManager = signinManager;
        _logger = logger;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<UserGetDto?> GetUserByIdAsync(int id)
    {
        var userEntities = await _userRepository.GetByIdAsync(id).ConfigureAwait(false);
        if (userEntities == null)
            return null;
        var user = _mapper.Map<UserGetDto>(userEntities);
        return user;
    }

    public async Task<UserGetDto?> GetUserProfileAsync()
    {
        var userId = _currentUserService.UserId; // Lấy ID người dùng từ dịch vụ hiện tại
        if (!userId.HasValue)
            return null;
        // Kiểm tra xem người dùng có tồn tại không
        var userEntities = await _userRepository.GetByIdAsync(userId.Value).ConfigureAwait(false);
        if (userEntities == null)
            throw new Exception("User not found");
        var user = _mapper.Map<UserGetDto>(userEntities);
        return user;
    }

    public async Task<RegisterResponseDTO> RegisterAsync(RegisterDTO request)
    {
        var userRole = await _roleRepository.GetUserRole().ConfigureAwait(false); // Lấy vai trò người dùng từ repository
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            Role = userRole, // Lấy vai trò người dùng từ repository
        };

        var result = await _userManager.CreateAsync(user, request.Password).ConfigureAwait(false);
        if (result.Succeeded)
        {
            var token = await _userManager
                .GenerateEmailConfirmationTokenAsync(user)
                .ConfigureAwait(false);
            var callbackUrl =
                $"{_configuration["AppSettings:ServerUrl"]}/api/account/confirm-email?email={Uri.EscapeDataString(request.Email)}&token={Uri.EscapeDataString(token)}";
            await _emailService
                .SendEmailAsync(request.Email, "Confirm your email", callbackUrl)
                .ConfigureAwait(false);
            return new RegisterResponseDTO
            {
                IsSuccess = true,
                Message =
                    "Registration successful. Please check your email to confirm your account.",
            };
        }

        return new RegisterResponseDTO
        {
            IsSuccess = false,
            Message = "Failed Register",
            Errors = result.Errors.Select(e => e.Description).ToList(),
        };
    }

    private async Task SignInWithCookies(User user, bool rememberMe)
    {
        var userWithRole = await _userRepository
            .GetUserWithRoleByIdAsync(user.Id)
            .ConfigureAwait(false); // Lấy tên vai trò của người dùng
        var roleName = userWithRole != null ? userWithRole.Role!.Name : "User"; // Lấy tên vai trò của người dùng
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Thêm thông tin ID người dùng
            new Claim(ClaimTypes.Name, user.UserName ?? "Unknown"), // Thêm tên người dùng
            new Claim(ClaimTypes.Email, user.Email ?? "NoEmail"), // Thêm email người dùng
            new Claim(ClaimTypes.Role, roleName ?? "User"), // Thêm vai trò người dùng
        };

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = rememberMe, // Kiểm tra xem người dùng có muốn giữ đăng nhập hay không
            ExpiresUtc = rememberMe
                ? DateTimeOffset.UtcNow.AddDays(7) // Nếu nhớ mật khẩu, thời gian hết hạn là 7 ngày
                : DateTimeOffset.UtcNow.AddHours(1), // Nếu không, hết hạn sau 1 giờ
        };

        // Đăng nhập người dùng và lưu thông tin vào cookie
        await _signinManager
            .SignInWithClaimsAsync(user, authProperties, claims)
            .ConfigureAwait(false);
    }

    public async Task<GoogleLoginResponse> GoogleAuthen(AuthenticateResult result)
    {
        // Nếu xác thực thất bại hoặc không có thông tin người dùng, trả về lỗi
        if (!result.Succeeded || result.Principal == null)
            return new GoogleLoginResponse
            {
                IsSuccess = false,
                Message = "Google authentication failed.",
                ErrorMessage = result.Failure?.Message ?? "No user information found.",
            };

        // Lấy thông tin người dùng từ Google
        var email = result.Principal.FindFirstValue(ClaimTypes.Email);
        var firstName = result.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "Unknown";
        var lastName = result.Principal.FindFirstValue(ClaimTypes.Surname) ?? "Unknown";
        var googleId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var avatarUrl = string.Empty;

        // Nếu có ảnh đại diện trong thông tin Google, lấy ảnh đại diện
        if (result.Principal.HasClaim(c => c.Type == "urn:google:picture"))
        {
            avatarUrl = result.Principal.FindFirst("urn:google:picture")?.Value ?? null;
        }
        if (email == null)
            return new GoogleLoginResponse
            {
                IsSuccess = false,
                Message = "Email not found.",
                ErrorMessage = "Email not found.",
            };

        // Lấy role người dùng
        var guestRole = await _roleRepository.GetUserRole().ConfigureAwait(false);

        // Kiểm tra xem người dùng đã tồn tại chưa
        var existingUser = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
        if (existingUser == null)
        {
            existingUser = CreateNewUser(
                $"{firstName} {lastName}",
                email,
                guestRole,
                avatarUrl: avatarUrl,
                confirmedEmail: true // Đánh dấu email đã được xác nhận
            );
            // Nếu người dùng chưa tồn tại, tạo người dùng mới
            await _userManager.CreateAsync(existingUser).ConfigureAwait(false);
        }
        else
        {
            // Nếu người dùng đã tồn tại, kiểm tra xem có cần cập nhật thông tin không
            if (existingUser.ImageUrl != avatarUrl)
            {
                existingUser.ImageUrl = avatarUrl ?? Constraint.Image.DefaultUserImageUrl; // Cập nhật ảnh đại diện nếu khác
                await _userManager.UpdateAsync(existingUser).ConfigureAwait(false);
            }
            if (existingUser.EmailConfirmed == false)
            {
                existingUser.EmailConfirmed = true; // Đánh dấu email đã được xác nhận
                await _userManager.UpdateAsync(existingUser).ConfigureAwait(false);
            }
            if (
                existingUser.LockoutEnabled
                && existingUser.LockoutEnd != null
                && existingUser.LockoutEnd > DateTime.UtcNow
            )
            {
                return new GoogleLoginResponse
                {
                    IsSuccess = false,
                    Message = "Google authentication failed.",
                    ErrorMessage = "User is locked out.",
                };
            }
        }
        await SignInWithCookies(existingUser, true).ConfigureAwait(false); // Đăng nhập người dùng với cookie

        var redirectUrl = $"{Constraint.Url.Client}/";
        return new GoogleLoginResponse
        {
            IsSuccess = true,
            Message = "Google authentication successful.",
            RedirectUrl = redirectUrl,
        };
    }

    public async Task<LoginResponseDTO> LoginAsync(LoginDTO request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
        if (user == null)
            return new LoginResponseDTO
            {
                IsSuccess = false,
                Message = "Login failed.",
                ErrorMessage = "Invalid password or Email.",
            };
        if (user.LockoutEnabled && user.LockoutEnd != null && user.LockoutEnd > DateTime.UtcNow)
            return new LoginResponseDTO
            {
                IsSuccess = false,
                Message = "Login failed.",
                ErrorMessage = "User is locked out.",
            };
        var result = await _signinManager
            .PasswordSignInAsync(user, request.Password, isPersistent: true, lockoutOnFailure: true)
            .ConfigureAwait(false);
        if (user.EmailConfirmed == false && result.Succeeded)
        {
            var token = await _userManager
                .GenerateEmailConfirmationTokenAsync(user)
                .ConfigureAwait(false);
            var callbackUrl =
                $"{_configuration["AppSettings:ServerUrl"]}/api/account/confirm-email?email={request.Email}&token={token}";
            await _emailService
                .SendEmailAsync(request.Email, "Confirm your email", callbackUrl)
                .ConfigureAwait(false);
            return new LoginResponseDTO
            {
                IsSuccess = false,
                Message = "Login failed.",
                ErrorMessage = "Email not confirmed. Please Confirm your Email",
            };
        }
        if (!result.Succeeded)
            return new LoginResponseDTO
            {
                IsSuccess = false,
                Message = "Login failed.",
                ErrorMessage = "Invalid password or Email.",
            };

        // Nếu đăng nhập thành công, tạo cookie cho người dùng
        await SignInWithCookies(user, false).ConfigureAwait(false); // Đăng nhập người dùng với cookie
        return new LoginResponseDTO { IsSuccess = true, Message = "Login successful." };
    }

    public async Task LogoutAsync()
    {
        // Đăng xuất người dùng và xóa cookie
        await _signinManager.SignOutAsync().ConfigureAwait(false);
    }

    public async Task<bool> VerifyEmailAsync(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
        if (user == null)
            return false;
        var decodedToken = Uri.UnescapeDataString(token); // Decode token từ URL
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken).ConfigureAwait(false);
        if (!result.Succeeded)
            _logger.LogError(
                "Email confirmation for token {token} failed for user {Email}: {Errors}",
                decodedToken,
                email,
                result.Errors.Select(e => e.Description + e.Code)
            );
        return result.Succeeded;
    }

    private User CreateNewUser(
        string FullName,
        string email,
        Role guestRole,
        DateTime? dateOfBirth = null,
        string gender = "Both",
        string? avatarUrl = null,
        bool confirmedEmail = false
    )
    {
        return new User
        {
            UserName = email,
            NormalizedUserName = FullName.ToUpper(),
            Email = email,
            NormalizedEmail = email.ToUpper(),
            Gender = gender,
            ImageUrl = avatarUrl ?? Constraint.Image.DefaultUserImageUrl, // Nếu không có ảnh đại diện, gán ảnh mặc định
            DateOfBirth = dateOfBirth ?? DateTime.Now, // Nếu không có ngày sinh, gán ngày hiện tại
            Role = guestRole,
            FullName = FullName,
            EmailConfirmed = confirmedEmail, // Nếu đã xác nhận email, gán true
        };
    }
}
