using System.ComponentModel.DataAnnotations;

public class CommentGetDto
{
    public int Id { get; set; }
    public string Text { get; set; } = "This is a blank comment";
    public int Rating { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public List<CommentImageGetDto> Images { get; set; } = new List<CommentImageGetDto>();
}

public class CommentCreateDto
{
    [Required(ErrorMessage = "Comment text is required")]
    [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
    public string Text { get; set; } = "This is a blank comment";

    [Required(ErrorMessage = "Rating is required")]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }

    [Required(ErrorMessage = "Product ID is required")]
    public int ProductId { get; set; }
    public List<CommentImageCreateDto> Images { get; set; } = new List<CommentImageCreateDto>();
}

public class CommentPutDto
{
    [Required(ErrorMessage = "Comment text is required")]
    [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
    public string Text { get; set; } = "This is a blank comment";

}
