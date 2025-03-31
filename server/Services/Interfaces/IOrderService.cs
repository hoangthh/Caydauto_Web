using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

public interface IOrderService
{
    Task<OrderResponse> CreateOrderAsync(OrderCreateDto order);
    Task<OrderResponse> UpdateOrderAsync(int orderId, string newStatus);
    Task<OrderGetDto> GetOrderAsync(int orderId);
    Task<List<OrderGetDto>> GetOrdersAsync(int pageNumber, int pageSize);
}
