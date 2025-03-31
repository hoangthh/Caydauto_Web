using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

public interface IOrderService { }

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDeliveryService _deliveryService;
    private readonly IAddressService _addressService;

    public OrderService(
        IDeliveryService deliveryService,
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IDiscountRepository discountRepository,
        ICurrentUserService currentUserService,
        IAddressService addressService
    )
    {
        _deliveryService = deliveryService;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _discountRepository = discountRepository;
        _currentUserService = currentUserService;
        _addressService = addressService;
    }

    public async Task<OrderResponse> CreateOrderAsync(OrderCreateDto order)
    {
        var userId = _currentUserService.UserId;
        if (userId == null)
        {
            return new OrderResponse { IsSuccess = false, Message = "User is not logged in" };
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
            var totalPriceAfterDiscount = totalPrice;
            // Tính phí vận chuyển (DeliveryFee)
            var deliveryFee = await _deliveryService
                .GetShippingFeeAsync(
                    order.ToDistrictId,
                    order.ToWardId,
                    (int)Math.Round(totalPrice, MidpointRounding.AwayFromZero)
                )
                .ConfigureAwait(false);

            // Xử lý mã giảm giá (nếu có) - chỉ tính toán, chưa trừ Quantity
            Discount? discount = null;
            decimal discountAmount = 0;
            if (!string.IsNullOrEmpty(order.DiscountCode))
            {
                discount = await _discountRepository
                    .GetDiscountByCodeAsync(order.DiscountCode)
                    .ConfigureAwait(false);

                if (discount == null || discount.Quantity <= 0)
                {
                    return new OrderResponse
                    {
                        IsSuccess = false,
                        Message = "Discount code is not valid",
                    };
                }

                string discountType = discount.Type;
                switch (discountType.ToUpper())
                {
                    case "PERCENTAGE":
                        totalPriceAfterDiscount = totalPrice * (1 - discount.Value / 100);
                        discountAmount = totalPrice - totalPriceAfterDiscount;
                        break;
                    case "FIXEDAMOUNT":
                        totalPriceAfterDiscount = totalPrice - discount.Value;
                        discountAmount = totalPriceAfterDiscount - totalPrice;
                        break;
                    case "SHIPPINGPERCENTAGE":
                        deliveryFee = (int)
                            Math.Round(
                                deliveryFee * (1 - discount.Value / 100),
                                MidpointRounding.AwayFromZero
                            );
                        break;
                    default:
                        break;
                }
            }
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
            var productsPrice = await _productRepository
                .GetProductPriceByIdsAsync(order.OrderItems.Select(o => o.ProductId).ToArray())
                .ConfigureAwait(false);
            var orderEntity = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                OrderStatus = "Pending", // Trạng thái ban đầu
                TotalPrice = totalPrice,
                TotalPriceAfterDiscount = totalPriceAfterDiscount,
                DeliveryFee = deliveryFee,
                DiscountId = discount == null ? null : discount.Id, // Lưu mã giảm giá để sử dụng sau
                ShippingAddress = addressResult.Message,
                DeliveryDiscount =
                    discount == null ? 0
                    : discount.Type.ToUpper() == "SHIPPINGPERCENTAGE"
                        ? (int)Math.Round(deliveryFee * discount.Value / 100)
                    : 0,
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
                // Các thuộc tính khác của Order
            };

            // Lưu đơn hàng vào database
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

    public async Task<OrderResponse> UpdateOrderAsync(int orderId, string newStatus)
    {
        using var transaction = await _orderRepository
            .BeginTransactionAsync()
            .ConfigureAwait(false);
        try
        {
            // Lấy đơn hàng từ database
            var order = await _orderRepository
                .GetByIdAsync(
                    orderId,
                    q =>
                    {
                        // Bao gồm các thực thể cấp một
                        var query = q.IncludeMultiple(o => o.User, o => o.Discount);

                        // Bao gồm OrderItems và các thực thể liên quan
                        return query
                            .Include(o => o.OrderItems)
                            .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.Colors)
                            .Include(o => o.OrderItems)
                            .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.Images);
                    }
                )
                .ConfigureAwait(false);
            if (order == null)
            {
                return new OrderResponse { IsSuccess = false, Message = "Order not found" };
            }

            // Chỉ xử lý nếu trạng thái mới là "Processing"
            if (newStatus.ToUpper() == "PROCESSING" && order.OrderStatus.ToUpper() != "PROCESSING")
            {
                // Lấy danh sách sản phẩm và số lượng từ OrderItems
                (int ProductId, int Quantity)[] productIdQuantity = order
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
                var productIds = productIdQuantity.Select(x => x.ProductId).ToList();
                var products = await _productRepository
                    .GetByIdsAsync(productIds)
                    .ConfigureAwait(false);

                // Tạo dictionary để tra cứu nhanh số lượng từ productIdQuantity
                var quantityDict = productIdQuantity.ToDictionary(
                    x => x.ProductId,
                    x => x.Quantity
                );
                // Cập nhật StockQuantity và Sold cho tất cả sản phẩm
                foreach (var product in products)
                {
                    if (quantityDict.TryGetValue(product.Id, out int quantity))
                    {
                        product.StockQuantity -= quantity;
                        product.Sold += quantity;
                    }
                }
                var result = await _productRepository
                    .UpdateRangeAsync(products)
                    .ConfigureAwait(false);
                if (!result)
                {
                    return new OrderResponse
                    {
                        IsSuccess = false,
                        Message = "Failed to update product stock quantity and sold",
                    };
                }
                if (order.DiscountId != null && order.DiscountId != 0)
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
}
