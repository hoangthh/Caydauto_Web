using Microsoft.AspNetCore.Identity;

public class Role : IdentityRole<int>
{
    public string Description { get; set; } = "This is a blank role";
    public ICollection<User> Users { get; set; } = new List<User>(); // Một Role có nhiều User
}
