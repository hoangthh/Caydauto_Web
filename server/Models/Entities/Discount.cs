using System.ComponentModel.DataAnnotations;

public class Discount
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Discount type is required")]
    [RegularExpression(
        "Percentage|FixedAmount|ShippingPercentage",
        ErrorMessage = "Invalid discount type"
    )]
    public string Type { get; set; } = "Percentage";

    [Required(ErrorMessage = "Discount value is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Discount value must be greater than or equal to 0")]
    public decimal Value { get; set; } = 0;

    [StringLength(50, ErrorMessage = "Discount code cannot exceed 50 characters")]
    public string Code { get; set; } = string.Empty;
    public int Quantity;
}
