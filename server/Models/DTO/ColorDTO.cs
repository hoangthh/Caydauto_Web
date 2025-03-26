using System.ComponentModel.DataAnnotations;

public class ColorGetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "Blank color";
}

public class ColorCreateDto
{
    [Required(ErrorMessage = "Color name is required")]
    [StringLength(20, ErrorMessage = "Color name cannot exceed 20 characters")]
    public string Name { get; set; } = "Blank color";
}

public class ColorPutDto
{
    [Required(ErrorMessage = "Color name is required")]
    [StringLength(20, ErrorMessage = "Color name cannot exceed 20 characters")]
    public string Name { get; set; } = "Blank color";
}
