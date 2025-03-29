using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Cart
{
    public int Id { get; set; }

    [Required(ErrorMessage = "User ID is required")]
    public int UserId { get; set; }

    // Navigation properties
    public User User { get; set; }
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
