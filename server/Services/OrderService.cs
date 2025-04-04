using AutoMapper;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDeliveryService _deliveryService;
    private readonly IMapper _mapper;
    private readonly IAddressService _addressService;

    public OrderService(
        IDeliveryService deliveryService,
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IDiscountRepository discountRepository,
        ICurrentUserService currentUserService,
        IAddressService addressService,
        IMapper mapper
    )
    {
        _deliveryService = deliveryService;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _discountRepository = discountRepository;
        _currentUserService = currentUserService;
        _addressService = addressService;
        _mapper = mapper;
    }

    public async Task<OrderResponse> CreateOrderAsync(OrderCreateDto order)
    {
        var userId = _currentUserService.UserId;
        if (userId == null)
        {
            return new OrderResponse { IsSuccess = false, Message = "User is not logged in" };
        }

        if(order == null || order.OrderItems == null || !order.OrderItems.Any())
        {
            return new OrderResponse { IsSuccess = false, Message = "Invalid order data" };
        }

        using var transaction = await _orderRepository
            .BeginTransactionAsync()
            .ConfigureAwait(false);
        try
        {
            // Lấy danh sách sản phẩm và số lượng từ OrderItems
            (int ProductId, int Quantity)[] productIdQuantity = order
                .OrderItems.Select(oi => (oi.ProductId, oi.Quantity))
                .ToArray();

            // Tính tổng giá sản phẩm (TotalPrice)
            var totalPrice = await _productRepository
                .GetTotalPriceByProductsIdAsync(productIdQuantity)
                .ConfigureAwait(false);

            // Tính phí vận chuyển (DeliveryFee)
            var deliveryFee = await _deliveryService
                .GetShippingFeeAsync(
                    order.ToDistrictId,
                    order.ToWardId,
                    totalPrice
                )
                .ConfigureAwait(false);

            // Áp dụng mã giảm giá
            var (totalPriceAfterDiscount, updatedDeliveryFee, discountAmount) =
                await ApplyDiscountAsync(totalPrice, deliveryFee, order.DiscountCode)
                    .ConfigureAwait(false);

            // Lấy địa chỉ giao hàng
            var addressResult = await _addressService
                .GetAddress(
                    order.ShippingAddress,
                    order.ToProvinceId,
                    order.ToDistrictId,
                    order.ToWardId
                )
                .ConfigureAwait(false);
            if (!addressResult.Success)
            {
                return new OrderResponse { IsSuccess = false, Message = addressResult.Message };
            }

            // Lấy giá sản phẩm
            var productsPrice = await _productRepository
                .GetProductPriceByIdsAsync(order.OrderItems.Select(o => o.ProductId).ToArray())
                .ConfigureAwait(false);

            // Tạo đơn hàng
            var orderEntity = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                OrderStatus = "Pending",
                TotalPrice = totalPrice,
                TotalPriceAfterDiscount = totalPriceAfterDiscount,
                DeliveryFee = updatedDeliveryFee,
                DiscountId = string.IsNullOrEmpty(order.DiscountCode)
                    ? null
                    : (
                        await _discountRepository
                            .GetDiscountByCodeAsync(order.DiscountCode)
                            .ConfigureAwait(false)
                    )?.Id,
                ShippingAddress = addressResult.Message,
                DeliveryDiscount =
                    updatedDeliveryFee != deliveryFee ? deliveryFee - updatedDeliveryFee : 0,
                DiscountAmount = discountAmount,
                OrderItems = order
                    .OrderItems.Select(oi => new OrderItem
                    {
                        ProductId = oi.ProductId,
                        ColorId = oi.ColorId,
                        Quantity = oi.Quantity,
                        UnitPrice = productsPrice[oi.ProductId],
                    })
                    .ToList(),
            };

            // Lưu đơn hàng
            await _orderRepository.AddAsync(orderEntity).ConfigureAwait(false);
            await transaction.CommitAsync().ConfigureAwait(false);

            return new OrderResponse
            {
                IsSuccess = true,
                Message = "Order created successfully with status Pending",
                OrderId = orderEntity.Id,
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync().ConfigureAwait(false);
            return new OrderResponse
            {
                IsSuccess = false,
                Message = $"Failed to create order: {ex.Message}",
            };
        }
    }

    public async Task<OrderGetDto?> GetOrderAsync(int orderId)
    {
        var order = await _orderRepository
            .GetByIdAsync(orderId, _orderRepository.OrderNavigate())
            .ConfigureAwait(false);
        if (order == null)
            return null;
        return MapToOrderGetDto(order);
    }

    public async Task<List<OrderGetDto>> GetOrdersAsync(int pageNumber, int pageSize)
    {
        var orders = await _orderRepository
            .GetAllAsync(pageNumber, pageSize, true, _orderRepository.OrderNavigate())
            .ConfigureAwait(false);
        return MapToOrderGetDtos(orders.Items);
    }

    public async Task<OrderResponse> UpdateOrderAsync(int orderId, string newStatus)
    {
        using var transaction = await _orderRepository
            .BeginTransactionAsync()
            .ConfigureAwait(false);
        try
        {
            // Lấy đơn hàng từ database
            var order = await _orderRepository
                .GetByIdAsync(orderId, _orderRepository.OrderNavigate())
                .ConfigureAwait(false);
            if (order == null)
            {
                return new OrderResponse { IsSuccess = false, Message = "Order not found" };
            }

            // Chỉ xử lý nếu trạng thái mới là "Processing"
            if (newStatus.ToUpper() == "PROCESSING" && order.OrderStatus.ToUpper() != "PROCESSING")
            {
                // Lấy danh sách sản phẩm và số lượng từ OrderItems
                var productIdQuantity = order
                    .OrderItems.Select(oi => (oi.ProductId, oi.Quantity))
                    .ToArray();

                // Kiểm tra số lượng tồn kho
                var productsNotQualified = await _productRepository
                    .CheckQuantityProducts(productIdQuantity)
                    .ConfigureAwait(false);

                if (productsNotQualified.Any())
                {
                    return new OrderResponse
                    {
                        IsSuccess = false,
                        Message =
                            $"These products do not have enough quantity: {string.Join(", ", productsNotQualified)}",
                    };
                }

                // Cập nhật StockQuantity và Sold
                var updateResult = await UpdateProductQuantitiesAsync(
                        productIdQuantity,
                        isCancellation: false
                    )
                    .ConfigureAwait(false);
                if (!updateResult)
                {
                    return new OrderResponse
                    {
                        IsSuccess = false,
                        Message = "Failed to update product stock quantity and sold",
                    };
                }

                // Trừ Discount.Quantity nếu có mã giảm giá
                if (order.DiscountId.HasValue)
                {
                    var discount = await _discountRepository
                        .GetByIdAsync(order.DiscountId.Value)
                        .ConfigureAwait(false);

                    if (discount != null && discount.Quantity > 0)
                    {
                        discount.Quantity--;
                        await _discountRepository.UpdateAsync(discount).ConfigureAwait(false);
                    }
                    else
                    {
                        return new OrderResponse
                        {
                            IsSuccess = false,
                            Message = "Discount code is no longer valid",
                        };
                    }
                }
            }

            // Cập nhật trạng thái đơn hàng
            order.OrderStatus = newStatus;
            await _orderRepository.UpdateAsync(order).ConfigureAwait(false);

            await transaction.CommitAsync().ConfigureAwait(false);

            return new OrderResponse
            {
                IsSuccess = true,
                Message = $"Order updated to status {newStatus} successfully",
                OrderId = order.Id,
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync().ConfigureAwait(false);
            return new OrderResponse
            {
                IsSuccess = false,
                Message = $"Failed to update order: {ex.Message}",
            };
        }
    }

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
                deliveryFee = 
                        deliveryFee * (1 - discount.Value / 100);
                break;
        }

        return (totalPriceAfterDiscount, deliveryFee, discountAmount);
    }

    private async Task<bool> UpdateProductQuantitiesAsync(
        IEnumerable<(int ProductId, int Quantity)> productIdQuantity,
        bool isCancellation = false
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

    private OrderGetDto MapToOrderGetDto(Order order)
    {
        return _mapper.Map<OrderGetDto>(order);
    }

    private List<OrderGetDto> MapToOrderGetDtos(IEnumerable<Order> orders)
    {
        return _mapper.Map<List<OrderGetDto>>(orders);
    }

    public async Task<OrderResponse> CancelOrderAsync(int orderId)
    {
        using var transaction = await _orderRepository
            .BeginTransactionAsync()
            .ConfigureAwait(false);
        try
        {
            // Lấy đơn hàng từ database
            var order = await _orderRepository
                .GetByIdAsync(orderId, _orderRepository.OrderNavigate())
                .ConfigureAwait(false);
            if (order == null)
            {
                return new OrderResponse { IsSuccess = false, Message = "Order not found" };
            }

            // Kiểm tra trạng thái đơn hàng
            var currentStatus = order.OrderStatus.ToUpper();
            if (currentStatus != "PENDING" && currentStatus != "PROCESSING")
            {
                return new OrderResponse
                {
                    IsSuccess = false,
                    Message = "Cannot cancel order. Order must be in Pending or Processing status.",
                };
            }

            // Nếu trạng thái là Processing, hoàn lại số lượng sản phẩm
            if (currentStatus == "PROCESSING")
            {
                var productIdQuantity = order
                    .OrderItems.Select(oi => (oi.ProductId, oi.Quantity))
                    .ToArray();

                var updateResult = await UpdateProductQuantitiesAsync(
                        productIdQuantity,
                        isCancellation: true
                    )
                    .ConfigureAwait(false);
                if (!updateResult)
                {
                    return new OrderResponse
                    {
                        IsSuccess = false,
                        Message = "Failed to restore product stock quantity and sold",
                    };
                }
            }

            // Hoàn lại Discount.Quantity nếu có mã giảm giá
            if (order.DiscountId.HasValue)
            {
                var discount = await _discountRepository
                    .GetByIdAsync(order.DiscountId.Value)
                    .ConfigureAwait(false);

                if (discount != null)
                {
                    discount.Quantity++;
                    await _discountRepository.UpdateAsync(discount).ConfigureAwait(false);
                }
                else
                {
                    return new OrderResponse { IsSuccess = false, Message = "Discount not found" };
                }
            }

            // Cập nhật trạng thái đơn hàng thành Cancelled
            order.OrderStatus = "Cancelled";
            await _orderRepository.UpdateAsync(order).ConfigureAwait(false);

            await transaction.CommitAsync().ConfigureAwait(false);

            return new OrderResponse
            {
                IsSuccess = true,
                Message = "Order cancelled successfully",
                OrderId = order.Id,
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync().ConfigureAwait(false);
            return new OrderResponse
            {
                IsSuccess = false,
                Message = $"Failed to cancel order: {ex.Message}",
            };
        }
    }
}
