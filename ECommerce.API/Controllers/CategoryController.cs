using ECommerce.Application.DTOs.Categories;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Runtime.InteropServices;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryService.GetAllAsync();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetByIdAsync(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> AddAsync(
            CreateCategoryDto createCategoryDto)
        {
            var category = await _categoryService.AddAsync(createCategoryDto);

            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateAsync(
            int id,
            UpdateCategoryDto updateCategoryDto)
        {
            var category = await _categoryService.UpdateAsync(
                id,
                updateCategoryDto);

            if (category == null)
            {
                return NotFound($"Category with ID {id} was not found.");
            }

            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound($"Category with ID {id} was not found.");
            }
           
            return NoContent();
        }
    }
}
