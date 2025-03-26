using System.ComponentModel.DataAnnotations;

public class UserGetDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserImageUrl { get; set; } = Constraint.Image.DefaultUserImageUrl;
    public string DateOfBirth { get; set; } = string.Empty;
}

public class UserCreateDto
{
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(
        100,
        MinimumLength = 2,
        ErrorMessage = "Full name must be between 2 and 100 characters"
    )]
    public string FullName { get; set; } = "Mysterious user";

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Username is required")]
    [StringLength(
        50,
        MinimumLength = 3,
        ErrorMessage = "Username must be between 3 and 50 characters"
    )]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(
        100,
        MinimumLength = 6,
        ErrorMessage = "Password must be between 6 and 100 characters"
    )]
    public string Password { get; set; } = string.Empty;
}

public class UserPutDto
{
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(
        100,
        MinimumLength = 2,
        ErrorMessage = "Full name must be between 2 and 100 characters"
    )]
    public string FullName { get; set; } = "Mysterious user";

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Username is required")]
    [StringLength(
        50,
        MinimumLength = 3,
        ErrorMessage = "Username must be between 3 and 50 characters"
    )]
    public string UserName { get; set; } = string.Empty;
}
