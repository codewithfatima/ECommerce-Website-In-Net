using ECommerce.Application.DTOs.Categories;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class CategoryServices:ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryServices(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();

            var categoriesDto = categories.Select(c=> new CategoryDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
            }).ToList();

            
            return categoriesDto;
        }

   
        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);

           if(category == null)
            {
                return null;
            }

            return new CategoryDto{
               CategoryId = category.CategoryId,
               Name = category.Name
            };
        }

        public async Task<CreateCategoryDto> AddAsync(CreateCategoryDto createCategoryDto)
        {
            var category = new Category
            {
                Name = createCategoryDto.Name,
            };
            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();

            var categoryDto = new CreateCategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };

            return categoryDto;
        }

        public async Task<UpdateCategoryDto> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await _repository.GetByIdAsync(id );

            if(category == null)
            {
                throw new Exception($"Category {id} not found.");
            }

            category.Name = updateCategoryDto.Name;

            await _repository.UpdateAsync(category);
            await _repository.SaveChangesAsync();
            return new UpdateCategoryDto { CategoryId = category.CategoryId, Name = category.Name };

        }
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            if(category == null)
            {
             return false;
            }
            await  _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();

            return true;
        }
        public async Task SaveChangesAsync() 
            => await _repository.SaveChangesAsync();
    }
}
