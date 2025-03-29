using System.ComponentModel.DataAnnotations;

public class RegisterDTO
{
    public string FullName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [Required(ErrorMessage = "Email is required")]
    [StringLength(100, ErrorMessage = "Email length must be less than 100 characters")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
} 

public class RegisterResponseDTO
{
    public bool IsSuccess { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new List<string>();
}

public class LoginResponseDTO
{
    public bool IsSuccess { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}

public class GoogleLoginResponse : LoginResponseDTO
{
    public string RedirectUrl { get; set; } = string.Empty;
}

public class LoginDTO
{
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [Required(ErrorMessage = "Email is required")]
    [StringLength(100, ErrorMessage = "Email length must be less than 100 characters")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
