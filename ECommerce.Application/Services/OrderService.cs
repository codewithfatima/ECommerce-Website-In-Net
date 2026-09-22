using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Routing;
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
        public async Task<OrderDto> PlaceOrderAsync(string userId, CreateOrderDto dto)
        {
            var order = new Order();
            order.UserId = userId;
            order.Status = OrderStatus.Pending;
            order.OrderDate = DateTime.UtcNow;

            var productIds= dto.Items.Select(i=> i.ProductId).ToList();

            var products = await _productRepository.GetByIdsAsync(productIds);

            foreach (var item in dto.Items)
            {
                var product = products.FirstOrDefault(p=> p.Id == item.ProductId);

                if (product == null)
                    throw new Exception($"Product {item.ProductId} not found.");

                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    Product = product
                });
            }
           order.TotalAmount = order.Items.Sum(i=> i.Quantity * i.UnitPrice);
            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();
            var orderDto = new OrderDto
            {
                Id = order.Id,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    ProductImage = i.Product?.ProductImage?? "",  
                    ProductName = i.Product?.Name?? "",
                    Subtotal = i.Quantity * i.UnitPrice

                }).ToList()
            };
            return orderDto;
        }
        public async Task<List<OrderDto>> GetMyOrdersAsync(string userId)
        {
            var orders = await _orderRepository.GetByUserId(userId);

            var dtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                OrderDate = o.OrderDate,
                Items = o.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    ProductImage = i.Product?.ProductImage ?? "",
                    ProductName = i.Product?.Name ?? "",
                    Subtotal  = i.Quantity * i.UnitPrice
                }).ToList()

            }).ToList();

            return dtos;
        }
        public async Task<OrderDto?> GetOrderAsync(int id, string userId)
        {
            var order = await _orderRepository.GetById(id);

            if (order == null)
                return null;

            if (order.UserId != userId)
            {
                return null; 
            }

            return new OrderDto
            {
                Id = order.Id,
                Status = order.Status,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                CustomerName = order.User?.FullName ?? "" ,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? "",
                    ProductImage = i.Product?.ProductImage ?? "",
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Quantity * i.UnitPrice
                }).ToList()
            };
        }
        public async Task<bool> CancelOrderAsync(int id, string userId, string? reason)
        {
          var order = await _orderRepository.GetById(id);

            if (order == null)
                return false;

            if (order.UserId != userId)   
                return false;   

            if(order.Status != OrderStatus.Pending && order.Status != OrderStatus.Confirmed)
            {
                return false;
            }
           
            order.Status = OrderStatus.Cancelled;
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();
            return true;

        }
        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAll();

            var dtos = orders.Select(o=> new OrderDto
            {
                Id = o.Id,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                CustomerName = o.User?.FullName ?? "",
                OrderDate = o.OrderDate,
                Items = o.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    ProductImage = i.Product?.ProductImage ?? "",
                    ProductName = i.Product?.Name ?? "",
                    Subtotal = i.Quantity * i.UnitPrice
                }).ToList()
            }).ToList();

            return dtos;
        }


        public async Task<bool> UpdateStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetById(orderId);   // ← get ONE by id

            if (order == null) return false;
            order.Status = newStatus;

            await _orderRepository.UpdateAsync(order);
             await _orderRepository.SaveChangesAsync();
            return true;

        }
    }
}
