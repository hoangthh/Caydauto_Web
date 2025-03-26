using System.ComponentModel.DataAnnotations;

public class ImageGetDto
{
    public int Id { get; set; }
    public string Url { get; set; } = Constraint.Image.DefaultProductImageUrl;
    public int ProductId { get; set; }
}

public class ImageCreateDto
{
    public IFormFile? File { get; set; }

}

public class ImagePutDto
{
    public IFormFile? File { get; set; } // Dùng IFormFile để cập nhật tệp
}
