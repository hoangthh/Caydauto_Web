using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Comment
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Comment text is required")]
    [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
    public string Text { get; set; } = "This is a blank comment";

    [Required(ErrorMessage = "Rating is required")]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; } = 5;

    public int ProductId { get; set; }
    public int UserId { get; set; }

    // Navigation properties
    public Product Product { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<CommentImage> Images { get; set; } = new List<CommentImage>();
}
