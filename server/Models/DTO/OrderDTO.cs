using System.ComponentModel.DataAnnotations;

public class OrderGetDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = "COD";
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal ShippingDiscount { get; set; }
    public decimal TotalPrice { get; set; }
    public int? DiscountId { get; set; }
    public string OrderStatus { get; set; } = "Pending";
    public List<OrderItemGetDto> OrderItems { get; set; } = new List<OrderItemGetDto>();
}

public class OrderCreateDto
{

    [Required(ErrorMessage = "Payment method is required")]
    [RegularExpression("COD|VnPay", ErrorMessage = "Payment method must be COD or VnPay")]
    public string PaymentMethod { get; set; } = "COD";
    public string ShippingAddress { get; set; } = string.Empty;
    public int toProvinceId { get; set; }
    public int toDistrictId { get; set; }
    public string toWardId { get; set; } = string.Empty;
    

    // [Required(ErrorMessage = "Subtotal is required")]
    // [Range(0, double.MaxValue, ErrorMessage = "Subtotal must be greater than or equal to 0")]
    // public decimal SubTotal { get; set; }

    // [Range(0, double.MaxValue, ErrorMessage = "Discount amount must be greater than or equal to 0")]
    // public decimal DiscountAmount { get; set; }

    // [Range(0, double.MaxValue, ErrorMessage = "Shipping fee must be greater than or equal to 0")]
    // public decimal ShippingFee { get; set; }

    // [Range(
    //     0,
    //     double.MaxValue,
    //     ErrorMessage = "Shipping discount must be greater than or equal to 0"
    // )]
    // public decimal ShippingDiscount { get; set; }

    // [Required(ErrorMessage = "Order status is required")]
    // [RegularExpression(
    //     "Pending|Processing|Delivering|Delivered|Cancelled",
    //     ErrorMessage = "Invalid order status"
    // )]
    // public string OrderStatus { get; set; } = "Pending";

    public string? DiscountCode { get; set; }

    public List<OrderItemCreateDto> OrderItems { get; set; } = new List<OrderItemCreateDto>();
}


public class OrderResponse {
    public bool IsSuccess;
    public string? Message { get; set; }
    public List<OrderGetDto>? OrderItems { get; set; }
}