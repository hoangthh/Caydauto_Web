using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Order
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    [Required(ErrorMessage = "Order date is required")]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Shipping address is required")]
    [StringLength(200, ErrorMessage = "Shipping address cannot exceed 200 characters")]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Payment method is required")]
    [RegularExpression("COD|VnPay", ErrorMessage = "Payment method must be COD or VnPay")]
    public string PaymentMethod { get; set; } = "COD";

    [Required(ErrorMessage = "Subtotal is required")]
    public int TotalPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Discount amount must be greater than or equal to 0")]
    public decimal DiscountAmount { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Discount amount must be greater than or equal to 0")]
    public decimal TotalPriceAfterDiscount { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Shipping fee must be greater than or equal to 0")]
    public decimal DeliveryFee { get; set; }

    [Range(
        0,
        double.MaxValue,
        ErrorMessage = "Shipping discount must be greater than or equal to 0"
    )]
    public decimal DeliveryDiscount { get; set; } = 0;

    [Required(ErrorMessage = "Order status is required")]
    [RegularExpression(
        "Pending|Processing|Delivering|Delivered|Cancelled",
        ErrorMessage = "Invalid order status"
    )]
    public string OrderStatus { get; set; } = "Pending";
    public int? TransactionId { get; set; }

    public int? DiscountId { get; set; }

    // Navigation properties
    public User? User { get; set; }
    public Discount? Discount { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
