using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController:ControllerBase
    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) 
        {
            var order = await _orderService.GetByIdAsync(id);

            return Ok(order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UpdateOrderDto updateOrderDto)
        {
            var order = await _orderService.GetByIdAsync( id);

            if(order == null)
            {
                return NotFound($"Order {id} not found.");
            }
            await _orderService.UpdateAsync(id , updateOrderDto);
            return Ok("Order Updated successfully!");
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(CreateOrderDto createOrderDto)
        {
            await _orderService.AddAsync(createOrderDto);
            return Ok("Order Added Successfully!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if(order == null)
            {
                return NotFound($"Order {id} not found.");
            }
            await _orderService.DeleteAsync(id);
            return Ok("Order Delete Successfully!");
            
        }

    }
}
