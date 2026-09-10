using ECommerce.Application.DTOs.Products;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> AddAsync(CreateProductDto createProductDto);
        Task<ProductDto> UpdateAsync(int id, UpdateProductDto updateProductDto);
        Task DeleteAsync(int id);
        Task<List<ProductDto>> GetFilteredProductsAsync(string? name, string? category, string? sortBy, int pageNumber = 1, int pageSize = 12);
        Task<int> GetFilteredProductsCountAsync(string? name, string? category);
        Task SaveChangesAsync();
    }
}
