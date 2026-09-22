using ECommerce.Application.DTOs.Orders;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderService
    {
        // 1. Place order — customer
        Task<OrderDto> PlaceOrderAsync(string userId, CreateOrderDto dto);

        // 2. Get my orders — customer
        Task<List<OrderDto>> GetMyOrdersAsync(string userId);

        // 3. Get one order — owner only
        Task<OrderDto?> GetOrderAsync(int id, string userId);

        // 4. Cancel order — owner only
        Task<bool> CancelOrderAsync(int id, string userId, string? reason);

        // 5. Get all orders — admin only
        Task<List<OrderDto>> GetAllOrdersAsync();

        Task<bool> UpdateStatusAsync(int orderId, OrderStatus newStatus);
    }
}
