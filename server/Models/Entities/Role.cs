using Microsoft.AspNetCore.Identity;

public class Role : IdentityRole<int>
{
    public string Description { get; set; } = "This is a blank role";
}
