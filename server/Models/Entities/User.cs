using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<int>
{
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(
        100,
        MinimumLength = 2,
        ErrorMessage = "Full name must be between 2 and 100 characters"
    )]
    public string FullName { get; set; } = "Mysterious user";
    public string ImageUrl { get; set; } = Constraint.Image.DefaultUserImageUrl;
    public DateTime DateOfBirth { get; set; } = DateTime.Now;
    public string Gender { get; set; } = "Unknown";
    public int RoleId { get; set; } // Khóa ngoại đến Role

    // Navigation properties
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public Role? Role { get; set; }  // Navigation property
    public Cart Cart { get; set; } = new Cart(); // One-to-one với Cart
    public ICollection<Product> WishList { get; set; } = new List<Product>(); // Many-to-many với Product
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
