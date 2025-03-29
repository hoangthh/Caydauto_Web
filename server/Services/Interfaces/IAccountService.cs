using Microsoft.AspNetCore.Authentication;

public interface IAccountService
{
    Task<RegisterResponseDTO> RegisterAsync(RegisterDTO request);
    Task<LoginResponseDTO> LoginAsync(LoginDTO request);
    Task<GoogleLoginResponse> GoogleAuthen(AuthenticateResult result);
    Task LogoutAsync();
    Task<bool> VerifyEmailAsync(string token, string email);
}