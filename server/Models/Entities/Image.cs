using System.ComponentModel.DataAnnotations;

public class Image
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Image URL is required")]
    [StringLength(200, ErrorMessage = "Image URL cannot exceed 200 characters")]
    public string Url { get; set; } = Constraint.Image.DefaultProductImageUrl;

    public int ProductId { get; set; }

    // Navigation property
    public Product Product { get; set; } = new Product();
}
