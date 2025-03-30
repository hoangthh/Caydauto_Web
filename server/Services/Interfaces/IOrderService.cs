// public interface IOrderService { }

// public class OrderService : IOrderService
// {
//     private readonly IOrderRepository _orderRepository;
//     private readonly IProductRepository _productRepository;
//     private readonly IDiscountRepository _discountRepository;
//     private readonly ICurrentUserService _currentUserService;
//     private readonly IDeliveryService _deliveryService;
//     private readonly IAddressService _addressService;

//     public OrderService(
//         IDeliveryService deliveryService,
//         IProductRepository productRepository,
//         IOrderRepository orderRepository,
//         IDiscountRepository discountRepository,
//         ICurrentUserService currentUserService,
//         IAddressService addressService)
//     {
//         _deliveryService = deliveryService;
//         _orderRepository = orderRepository;
//         _productRepository = productRepository;
//         _discountRepository = discountRepository;
//         _currentUserService = currentUserService;
//         _addressService = addressService;
//     }

//     async Task<OrderResponse> CreateOrderAsync(OrderCreateDto order)
//     {
//         var userId = _currentUserService.UserId;
//         if(userId == null){
//             return new OrderResponse { IsSuccess = false, Message = "User is not logged in" };
//         }
//         Discount? discount;
//         using var transaction = await _orderRepository
//             .BeginTransactionAsync()
//             .ConfigureAwait(false);
//         try
//         {
//             (int, int)[] productIdQuantity = order
//                 .OrderItems.Select(oi => (oi.ProductId, oi.Quantity))
//                 .ToArray();
//             var productsNotQualified = await _productRepository
//                 .CheckQuantityProducts(productIdQuantity)
//                 .ConfigureAwait(false);
//             if (productsNotQualified.Any())
//             {
//                 return new OrderResponse
//                 {
//                     IsSuccess = false,
//                     Message =
//                         $"These products are not enough Quantity: {string.Join(", ", productsNotQualified)}",
//                 };
//             }
//             var TotalPrice = await _productRepository
//                 .GetTotalPriceByProductsIdAsync(productIdQuantity)
//                 .ConfigureAwait(false);
//             var deliveryFee = await _deliveryService
//                 .GetShippingFeeAsync(
//                     order.toDistrictId,
//                     order.toWardId,
//                     (int)Math.Round(TotalPrice, MidpointRounding.AwayFromZero)
//                 )
//                 .ConfigureAwait(false);
//             if (!String.IsNullOrEmpty(order.DiscountCode))
//             {
//                 discount = await _discountRepository
//                     .GetDiscountByCodeAsync(order.DiscountCode)
//                     .ConfigureAwait(false);
//                 if (discount == null || discount.Quantity <= 0)
//                 {
//                     return new OrderResponse
//                     {
//                         IsSuccess = false,
//                         Message = "Discount code is not valid",
//                     };
//                 }
//                 string discountType = discount.Type;
//                 switch (discountType.ToUpper())
//                 {
//                     case "PERCENTAGE":
//                         TotalPrice = TotalPrice * (1 - discount.Value / 100);
//                         break;
//                     case "FixedAmouth":
//                         TotalPrice = TotalPrice - discount.Value;
//                         break;
//                     case "ShippingPercentage":
//                         deliveryFee = (int)
//                             Math.Round(
//                                 deliveryFee * (1 - discount.Value / 100),
//                                 MidpointRounding.AwayFromZero
//                             );
//                         break;
//                     case null:
//                         break;
//                 }

//                 discount.Quantity--;
//                 await _discountRepository.UpdateAsync(discount).ConfigureAwait(false);
//             }
//             var orderEntity = new Order { 
//                 UserId = userId,
//                 OrderDate = DateTime.Now,
                
//             };

//             return new OrderResponse { };
//         }
//         catch (Exception ex)
//         {
//             await transaction.RollbackAsync().ConfigureAwait(false);
//             throw new Exception($"Failed to create Order: {ex.Message}");
//         }
//     }
// }
