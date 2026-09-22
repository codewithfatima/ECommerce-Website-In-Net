using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PlaceOrderAsync(CreateOrderDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var order = await _orderService.PlaceOrderAsync(userId, dto);

            return Ok(order);
        }

        [Authorize]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyOrdersAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var orders = await _orderService.GetMyOrdersAsync(userId!);

            return Ok(orders);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderAsync(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderService.GetOrderAsync(id, userId!);
            if (order == null)
                return NotFound();
            return Ok(order);

        }

        [Authorize]
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrderAsync(int id, CancelOrderDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var success = await _orderService.CancelOrderAsync(id, userId!, dto.Reason);

            if (!success)
                return NotFound();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateStatusDto dto)
        {
            var success = await _orderService.UpdateStatusAsync(id, dto.NewStatus);

            if (!success) return NotFound();

            return Ok();
        }

    }
}
