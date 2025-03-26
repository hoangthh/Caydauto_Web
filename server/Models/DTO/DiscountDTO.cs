using System.ComponentModel.DataAnnotations;

public class DiscountGetDto
{
    public int Id { get; set; }
    public string Type { get; set; } = "Percentage";
    public decimal Value { get; set; }
    public string Code { get; set; } = string.Empty;
}

public class DiscountCreateDto
{
    [Required(ErrorMessage = "Discount type is required")]
    [RegularExpression(
        "Percentage|FixedAmount|ShippingPercentage",
        ErrorMessage = "Invalid discount type"
    )]
    public required string Type { get; set; }

    [Required(ErrorMessage = "Discount value is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Discount value must be greater than or equal to 0")]
    public decimal Value { get; set; }

    [StringLength(50, ErrorMessage = "Discount code cannot exceed 50 characters")]
    public required string Code { get; set; }
}

public class DiscountPutDto
{
    [Required(ErrorMessage = "Discount type is required")]
    [RegularExpression(
        "Percentage|FixedAmount|ShippingPercentage",
        ErrorMessage = "Invalid discount type"
    )]
    public required string Type { get; set; }

    [Required(ErrorMessage = "Discount value is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Discount value must be greater than or equal to 0")]
    public decimal Value { get; set; }

    [StringLength(50, ErrorMessage = "Discount code cannot exceed 50 characters")]
    public required string Code { get; set; }
}
