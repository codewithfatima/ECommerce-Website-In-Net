using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async  Task<IActionResult> GetAllProductsAsync()
        {
            var products = await _service.GetAllProductsAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var products = await _service.GetProductByIdAsync(id);

            if(products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAsync(CreateProductDto createProductDto)
        {
           var products = await _service.AddAsync(createProductDto);
            return Ok(products);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync(int id, UpdateProductDto updateProductDto)
        {
            var product = await _service.UpdateAsync(id, updateProductDto);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
              await _service.DeleteAsync(id);
            return Ok("Product has been successfully deleted");
            
        }

        [HttpGet("search")]
        public async Task<IActionResult>  GetFilteredProductsAsync([FromQuery] string? name, string? category, string? sortBy, int pageNumber = 1, int pageSize = 12)
        {
            var products = await _service.GetFilteredProductsAsync(name ,category , sortBy , pageNumber, pageSize );

            return Ok(products);
        }

        [HttpGet("search/count")]
        public async Task<IActionResult> GetFilteredProductsCountAsync(string? name, string? category)
        {
            var count = await _service.GetFilteredProductsCountAsync(name, category);
            return Ok(count);
        }

    }
}
