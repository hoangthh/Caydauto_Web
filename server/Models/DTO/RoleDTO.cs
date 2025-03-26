using System.ComponentModel.DataAnnotations;

public class RoleGetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class RoleCreateDto
{
    [Required(ErrorMessage = "Role name is required")]
    [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
    public string Description { get; set; } = string.Empty;
} 

public class RolePutDto
{
    [Required(ErrorMessage = "Role name is required")]
    [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
    public string Description { get; set; } = string.Empty;
}
