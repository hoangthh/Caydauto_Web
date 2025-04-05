using System.Text.RegularExpressions;
using AutoMapper;

public interface IVnPayOrderService
{
    Task<(string PaymentUrl, OrderResponse OrderResult)> CreateOrderAndPaymentUrlAsync(
        HttpContext context,
        OrderCreateDto orderDto
    );
    Task<VnPaymentResponseModel> PaymentExecuteAsync(IQueryCollection collections);
}

public class VnPayOrderService : IVnPayOrderService
{
    private readonly IConfiguration _config;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDeliveryService _deliveryService;
    private readonly IAddressService _addressService;
    private readonly IMapper _mapper;

    public VnPayOrderService(
        IConfiguration configuration,
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IDiscountRepository discountRepository,
        ICurrentUserService currentUserService,
        IDeliveryService deliveryService,
        IAddressService addressService,
        IMapper mapper
    )
    {
        _config = configuration;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _discountRepository = discountRepository;
        _currentUserService = currentUserService;
        _deliveryService = deliveryService;
        _addressService = addressService;
        _mapper = mapper;
    }

    public async Task<(string PaymentUrl, OrderResponse OrderResult)> CreateOrderAndPaymentUrlAsync(
        HttpContext context,
        OrderCreateDto orderDto
    )
    {
        var userId = _currentUserService.UserId;
        if (userId == null)
        {
            return ("", new OrderResponse { IsSuccess = false, Message = "User is not logged in" });
        }

        if (orderDto == null || orderDto.OrderItems == null || !orderDto.OrderItems.Any())
        {
            return ("", new OrderResponse { IsSuccess = false, Message = "Invalid order data" });
        }

        using var transaction = await _orderRepository
            .BeginTransactionAsync()
            .ConfigureAwait(false);
        try
        {
            // Tính toán giá và phí
            var productIdQuantity = orderDto
                .OrderItems.Select(oi => (oi.ProductId, oi.Quantity))
                .ToArray();
            var totalPrice = await _productRepository
                .GetTotalPriceByProductsIdAsync(productIdQuantity)
                .ConfigureAwait(false);
            var deliveryFee = await _deliveryService
                .GetShippingFeeAsync(orderDto.ToDistrictId, orderDto.ToWardId, totalPrice)
                .ConfigureAwait(false);

            var (totalPriceAfterDiscount, updatedDeliveryFee, discountAmount) =
                await ApplyDiscountAsync(
                        totalPrice,
                        deliveryFee,
                        orderDto.DiscountCode ?? string.Empty
                    )
                    .ConfigureAwait(false);

            // Lấy địa chỉ
            var addressResult = await _addressService
                .GetAddress(
                    orderDto.ShippingAddress,
                    orderDto.ToProvinceId,
                    orderDto.ToDistrictId,
                    orderDto.ToWardId
                )
                .ConfigureAwait(false);

            if (!addressResult.Success)
            {
                return (
                    "",
                    new OrderResponse { IsSuccess = false, Message = addressResult.Message }
                );
            }

            // Tạo Order entity
            var productsPrice = await _productRepository
                .GetProductPriceByIdsAsync(orderDto.OrderItems.Select(o => o.ProductId).ToArray())
                .ConfigureAwait(false);

            var orderEntity = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                OrderStatus = "Pending",
                TotalPrice = totalPrice,
                TotalPriceAfterDiscount = totalPriceAfterDiscount,
                DeliveryFee = updatedDeliveryFee,
                DiscountId = string.IsNullOrEmpty(orderDto.DiscountCode)
                    ? null
                    : (
                        await _discountRepository
                            .GetDiscountByCodeAsync(orderDto.DiscountCode)
                            .ConfigureAwait(false)
                    )?.Id,
                ShippingAddress = addressResult.Message,
                DeliveryDiscount =
                    updatedDeliveryFee != deliveryFee ? deliveryFee - updatedDeliveryFee : 0,
                DiscountAmount = discountAmount,
                PaymentMethod = "VnPay",
                OrderItems = orderDto
                    .OrderItems.Select(oi => new OrderItem
                    {
                        ProductId = oi.ProductId,
                        ColorId = oi.ColorId,
                        Quantity = oi.Quantity,
                        UnitPrice = productsPrice[oi.ProductId],
                    })
                    .ToList(),
            };

            // Lưu order
            await _orderRepository.AddAsync(orderEntity).ConfigureAwait(false);
            await transaction.CommitAsync().ConfigureAwait(false);

            // Tạo payment URL
            var vnpay = new VnPayLibrary();
            var tick = DateTime.Now.Ticks.ToString();

            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"] ?? "2.1.0");
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"] ?? "pay");
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"] ?? "12345678");
            vnpay.AddRequestData("vnp_Amount", (totalPriceAfterDiscount * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"] ?? "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"] ?? "vn");
            vnpay.AddRequestData(
                "vnp_OrderInfo",
                $"Thanh toán đơn hàng {orderEntity.Id} với tổng giá {totalPriceAfterDiscount}"
            );
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData(
                "vnp_ReturnUrl",
                _config["VnPay:PaymentBackReturnUrl"]
                    ?? $"{Constraint.Url.Server}api/order/payment-callback"
            );
            vnpay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = vnpay.CreateRequestUrl(
                _config["VnPay:BaseUrl"] ?? "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
                _config["VnPay:HashSecret"] ?? "12345678"
            );

            return (
                paymentUrl,
                new OrderResponse
                {
                    IsSuccess = true,
                    Message = "Order created and payment URL generated",
                    OrderId = orderEntity.Id,
                }
            );
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync().ConfigureAwait(false);
            return (
                "",
                new OrderResponse
                {
                    IsSuccess = false,
                    Message = $"Failed to create order and payment: {ex.Message}",
                }
            );
        }
    }

    public async Task<VnPaymentResponseModel> PaymentExecuteAsync(IQueryCollection collections)
    {
        var vnpay = new VnPayLibrary();

        foreach (var (key, value) in collections)
        {
            if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
            {
                vnpay.AddResponseData(key, value.ToString());
            }
        }

        var vnp_orderId = ExtractOrderId(vnpay.GetResponseData("vnp_OrderInfo"));
        var vnp_TransactionId = vnpay.GetResponseData("vnp_TransactionId");
        var vnp_SecureHash = collections.FirstOrDefault(x => x.Key == "vnp_SecureHash").Value;
        var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
        var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

        bool checkSignature = vnpay.ValidateSignature(
            vnp_SecureHash!,
            _config["VnPay:HashSecret"] ?? string.Empty
        );
        if (!checkSignature || vnp_orderId == 0)
        {
            return new VnPaymentResponseModel { Success = false };
        }

        using var transaction = await _orderRepository
            .BeginTransactionAsync()
            .ConfigureAwait(false);
        try
        {
            var order = await _orderRepository.GetByIdAsync(vnp_orderId).ConfigureAwait(false);
            if (order == null || order.OrderStatus != "Pending")
            {
                return new VnPaymentResponseModel
                {
                    Success = false,
                    Message = "Order not found or not pending",
                };
            }

            // Xử lý logic khi thanh toán thành công
            if (vnp_ResponseCode == "00") // 00 là mã thành công của VnPay
            {
                // Trừ số lượng discount nếu có
                if (order.DiscountId.HasValue)
                {
                    var discount = await _discountRepository
                        .GetByIdAsync(order.DiscountId.Value)
                        .ConfigureAwait(false);
                    if (discount == null || discount.Quantity <= 0)
                    {
                        await RollbackOrderAsync(order.Id).ConfigureAwait(false);
                        return new VnPaymentResponseModel
                        {
                            Success = false,
                            Message = "Discount invalid",
                        };
                    }
                    discount.Quantity--;
                    await _discountRepository.UpdateAsync(discount).ConfigureAwait(false);
                }

                // Trừ số lượng sản phẩm
                var productIdQuantity = order
                    .OrderItems.Select(oi => (oi.ProductId, oi.Quantity))
                    .ToArray();
                var productsNotQualified = await _productRepository
                    .CheckQuantityProducts(productIdQuantity)
                    .ConfigureAwait(false);

                if (productsNotQualified.Any())
                {
                    await RollbackOrderAsync(order.Id).ConfigureAwait(false);
                    return new VnPaymentResponseModel
                    {
                        Success = false,
                        Message =
                            $"Not enough quantity for: {string.Join(", ", productsNotQualified)}",
                    };
                }

                await UpdateProductQuantitiesAsync(productIdQuantity, false).ConfigureAwait(false);
                order.OrderStatus = "Processing";
                order.TransactionId = int.TryParse(vnp_TransactionId, out int transId)
                    ? transId
                    : null;
            }
            else
            {
                await RollbackOrderAsync(order.Id).ConfigureAwait(false);
                return new VnPaymentResponseModel { Success = false, Message = "Payment failed" };
            }

            await _orderRepository.UpdateAsync(order).ConfigureAwait(false);
            await transaction.CommitAsync().ConfigureAwait(false);

            return new VnPaymentResponseModel
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_orderId.ToString(),
                TransactionId = vnp_TransactionId,
                Token = vnp_SecureHash!,
                VnPayResponseCode = vnp_ResponseCode,
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync().ConfigureAwait(false);
            return new VnPaymentResponseModel
            {
                Success = false,
                Message = $"Payment processing failed: {ex.Message}",
            };
        }
    }

    private async Task RollbackOrderAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId).ConfigureAwait(false);
        if (order != null)
        {
            order.OrderStatus = "Cancelled";
            await _orderRepository.UpdateAsync(order).ConfigureAwait(false);
        }
    }

    private int ExtractOrderId(string orderInfo)
    {
        string pattern = @"\b\d+\b";
        Match match = Regex.Match(orderInfo, pattern);
        return match.Success ? int.Parse(match.Value) : 0;
    }

    // Copy các phương thức helper từ OrderService
    private async Task<(
        decimal TotalPriceAfterDiscount,
        int DeliveryFee,
        decimal DiscountAmount
    )> ApplyDiscountAsync(decimal totalPrice, int deliveryFee, string discountCode)
    {
        decimal totalPriceAfterDiscount = totalPrice;
        decimal discountAmount = 0;

        if (string.IsNullOrEmpty(discountCode))
        {
            return (totalPriceAfterDiscount, deliveryFee, discountAmount);
        }

        var discount = await _discountRepository
            .GetDiscountByCodeAsync(discountCode)
            .ConfigureAwait(false);

        if (discount == null || discount.Quantity <= 0)
        {
            throw new Exception("Discount code is not valid");
        }

        switch (discount.Type.ToUpper())
        {
            case "PERCENTAGE":
                totalPriceAfterDiscount = totalPrice * (1 - discount.Value / 100);
                discountAmount = totalPrice - totalPriceAfterDiscount;
                break;
            case "FIXEDAMOUNT":
                totalPriceAfterDiscount = totalPrice - discount.Value;
                discountAmount = totalPrice - totalPriceAfterDiscount;
                break;
            case "SHIPPINGPERCENTAGE":
                deliveryFee = deliveryFee * (1 - discount.Value / 100);
                break;
        }

        return (totalPriceAfterDiscount, deliveryFee, discountAmount);
    }

    private async Task<bool> UpdateProductQuantitiesAsync(
        IEnumerable<(int ProductId, int Quantity)> productIdQuantity,
        bool isCancellation
    )
    {
        var productIds = productIdQuantity.Select(x => x.ProductId).ToList();
        var products = await _productRepository.GetByIdsAsync(productIds).ConfigureAwait(false);

        var quantityDict = productIdQuantity.ToDictionary(x => x.ProductId, x => x.Quantity);

        foreach (var product in products)
        {
            if (quantityDict.TryGetValue(product.Id, out int quantity))
            {
                if (isCancellation)
                {
                    product.StockQuantity += quantity;
                    product.Sold -= quantity;
                }
                else
                {
                    product.StockQuantity -= quantity;
                    product.Sold += quantity;
                }
            }
        }

        return await _productRepository.UpdateRangeAsync(products).ConfigureAwait(false);
    }
}
