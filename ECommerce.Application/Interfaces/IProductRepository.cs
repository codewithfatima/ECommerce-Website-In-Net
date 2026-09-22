using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> GetByIdsAsync(List<int> ids);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<List<Product>> GetFilteredProductsAsync( string? name, string? category , string? sortBy, int pageNumber = 1, int pageSize = 12);
        Task<int> GetFilteredProductsCountAsync(string? name, string? category);
        Task SaveChangesAsync();
    }
}
