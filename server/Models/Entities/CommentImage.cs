using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Endpoints;

public class CommentImage
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Image URL is required")]
    [StringLength(200, ErrorMessage = "Image URL cannot exceed 200 characters")]
    public string Url { get; set; } = Constraint.Image.DefaultProductImageUrl;

    public int CommentId { get; set; }

    // Navigation property
    public Comment? Comment { get; set; }
}
