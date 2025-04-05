public class VnPaymentResponseModel
{
    public bool Success { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string OrderDescription { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string PaymentId { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string VnPayResponseCode { get; set; } = string.Empty;
    public string? Message { get; set; }
}

public class VnPaymentRequestModel
{
    public string FullName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Amount { get; set; }
    public DateTime CreateDate { get; set; }
    public int OrderId { get; set; }
}
