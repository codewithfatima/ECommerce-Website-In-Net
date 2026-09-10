using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.DTOs.Orders;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(int id);
        Task<OrderDto> AddAsync(CreateOrderDto createOrderDto);
        Task UpdateAsync(int id, UpdateOrderDto updateOrderDto);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();


    }
}
