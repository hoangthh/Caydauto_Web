using System.ComponentModel.DataAnnotations;

public class CommentImageGetDto
{
    public int Id { get; set; }
    public string Url { get; set; } = Constraint.Image.DefaultProductImageUrl;
    public int CommentId { get; set; }
}

public class CommentImageCreateDto
{
    public IFormFile? File { get; set; } // Dùng IFormFile để tải tệp lên

}

public class CommentImagePutDto
{
    public IFormFile? File { get; set; }
}
