using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
            => await _context.Categories.ToListAsync();
        public async Task<Category> GetByIdAsync(int id)
        {
            var entity = await _context.Categories.FindAsync(id);

            if(entity == null)
            {
                throw new Exception("Category with this id not found");
            }
            return entity;
        }
           
        public async Task AddAsync(Category category)
            => await _context.Categories.AddAsync(category);
        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

        }
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Categories.FindAsync(id);


            if (entity == null)
            {
                return false;
            }
            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
