using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IVnPayOrderService _vnPayOrderService;

    public OrderController(IOrderService orderService, IVnPayOrderService vnPayOrderService)
    {
        _vnPayOrderService = vnPayOrderService;
        _orderService = orderService;
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetOrdersByUserId([FromQuery] int pageNumber, int pageSize)
    {
        var orders = await _orderService
            .GetOrdersByUserIdAsync(pageNumber, pageSize)
            .ConfigureAwait(false);
        return Ok(orders);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllOrders([FromQuery] int pageNumber, int pageSize)
    {
        var orders = await _orderService.GetOrdersAsync(pageNumber, pageSize).ConfigureAwait(false);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderService.GetOrderAsync(id).ConfigureAwait(false);
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Xử lý dựa trên PaymentMethod
        switch (orderCreateDto.PaymentMethod.ToUpper())
        {
            case "COD":
                var orderResult = await _orderService
                    .CreateOrderAsync(orderCreateDto)
                    .ConfigureAwait(false);
                if (!orderResult.IsSuccess)
                {
                    return BadRequest(new { Success = false, Message = orderResult.Message });
                }
                return Ok(
                    new
                    {
                        Success = true,
                        Message = orderResult.Message,
                        OrderId = orderResult.OrderId,
                    }
                );

            case "VNPAY":
                var (paymentUrl, vnPayOrderResult) = await _vnPayOrderService
                    .CreateOrderAndPaymentUrlAsync(HttpContext, orderCreateDto)
                    .ConfigureAwait(false);

                if (!vnPayOrderResult.IsSuccess)
                {
                    return BadRequest(new { Success = false, Message = vnPayOrderResult.Message });
                }
                return Ok(
                    new
                    {
                        Success = true,
                        Message = vnPayOrderResult.Message,
                        OrderId = vnPayOrderResult.OrderId,
                        PaymentUrl = paymentUrl,
                    }
                );

            default:
                return BadRequest(
                    new
                    {
                        Success = false,
                        Message = "Invalid payment method. Must be COD or VnPay",
                    }
                );
        }
    }

    [HttpPut("{id}/{newStatus}")]
    public async Task<IActionResult> UpdateOrder(int id, string newStatus)
    {
        var order = await _orderService.UpdateOrderAsync(id, newStatus).ConfigureAwait(false);
        if (!order.IsSuccess)
        {
            return BadRequest(order.Message);
        }
        return Ok(order);
    }

    [HttpGet("payment-callback")]
    public async Task<IActionResult> PaymentCallback()
    {
        var response = await _vnPayOrderService
            .PaymentExecuteAsync(HttpContext.Request.Query)
            .ConfigureAwait(false);

        if (!response.Success)
        {
            return BadRequest(
                new { Success = false, Message = response.Message ?? "Payment processing failed" }
            );
        }

        return Ok(
            new
            {
                Success = true,
                OrderId = response.OrderId,
                TransactionId = response.TransactionId,
                Message = "Payment processed successfully",
            }
        );
    }
}
