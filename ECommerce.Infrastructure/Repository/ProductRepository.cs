using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECommerce.Infrastructure.Repository
{
    public class ProductRepository:IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
            => await _context.Products.ToListAsync();

        public async Task<List<Product>> GetFilteredProductsAsync( string? name , string? category , string?  sortBy , int pageNumber = 1 , int pageSize = 12 )
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();


            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.Name == category);
            }
            if(sortBy == "price_asc")
            {
                query = query.OrderBy(p => p.Price);
            }
            else if(sortBy == "price_desc")
            {
                query = query.OrderByDescending(p => p.Price);
            }
            query = query.Skip((pageNumber -1 ) * pageSize).Take(pageSize);

                return await query.ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if(product == null)
            {
                throw new Exception("Product with this id is not fOund");
            }
            return product;
        }
        public async Task AddAsync(Product product)
            => await _context.Products.AddAsync(product);
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity != null)
            {
                _context.Products.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetFilteredProductsCountAsync(string? name, string? category)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if(!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }
            if(!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.Name == category);
            }
            return await query.CountAsync();
        }
        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
