using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class OrderService:IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;


        public OrderService(IOrderRepository orderRepository , IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAll();

            return orders.Select(order=> new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
            });

        }
        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var orders = await _orderRepository.GetById(id);

            if(orders == null)
            {
                return null;
            }

            return new OrderDto
            {
                Id = orders.Id,
                OrderDate = orders.OrderDate,
                Status = orders.Status,
                TotalAmount = orders.TotalAmount,
            };

        }
        public async Task<OrderDto> AddAsync(CreateOrderDto createOrderDto)
        {
            var order = new Order
            {
                CustomerId = createOrderDto.CustomerId,
                OrderDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Status = OrderStatus.Pending,
                TotalAmount = 0
            };
            
            foreach(var item in createOrderDto.Items)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = product.Price * item.Quantity,
                };
                order.OrderItems.Add(orderItem);
                order.TotalAmount += orderItem.TotalPrice;
            }
           await   _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
            };
        }
        public async Task UpdateAsync(int id, UpdateOrderDto updateOrderDto)
        {
            var exisitingOrder = await _orderRepository.GetById(id);

            if(exisitingOrder == null)
            {
                throw new Exception($"Order with this {id} is not found");
            }

            exisitingOrder.OrderDate = updateOrderDto.OrderDate;
            exisitingOrder.Status = updateOrderDto.Status;

            await _orderRepository.UpdateAsync(exisitingOrder);
            await _orderRepository.SaveChangesAsync();

        }
        public async Task DeleteAsync(int id)
        {
            var deletOrder = await _orderRepository.GetById(id);

            if(deletOrder != null)
            {
                await _orderRepository.DeleteAsync(deletOrder);
                await _orderRepository.SaveChangesAsync();
            }
           
        }
        public async Task SaveChangesAsync()
            => await _orderRepository.SaveChangesAsync();
    }
}
