using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
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
        var order = await _orderService.CreateOrderAsync(orderCreateDto).ConfigureAwait(false);
        if (!order.IsSuccess)
        {
            return BadRequest(order.Message);
        }
        return Ok(order);
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
}
