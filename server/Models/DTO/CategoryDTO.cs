using System.ComponentModel.DataAnnotations;

public class CategoryGetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "Blank category";
    public string Description { get; set; } = "This is a blank category";
}

public class CategoryCreateDto
{
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
    public string Name { get; set; } = "Blank category";

    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
    public string Description { get; set; } = "This is a blank category";
}

public class CategoryPutDto
{
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
    public string Name { get; set; } = "Blank category";

    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
    public string Description { get; set; } = "This is a blank category";
}
