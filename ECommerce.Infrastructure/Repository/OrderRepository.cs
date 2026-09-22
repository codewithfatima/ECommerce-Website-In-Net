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
    public class OrderRepository:IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAll()
            => await _context.Orders.Include(u => u.User).Include(o=>o.Items).ThenInclude(i=>i.Product).ToListAsync();
        public async Task<List<Order>> GetByUserId(string userId)
            => await _context.Orders.Include(o => o.Items).ThenInclude(i => i.Product).Where(o=>o.UserId == userId).ToListAsync();
        public async Task<Order?> GetById(int id)
            => await _context.Orders.Include(u => u.User).Include(o => o.Items).ThenInclude(i => i.Product).FirstOrDefaultAsync(o => o.Id == id);
        public async Task AddAsync(Order order)
            => await _context.Orders.AddAsync(order);
        public async Task UpdateAsync(Order order)
            => _context.Orders.Update(order);
        public async Task DeleteAsync(Order order)
            => _context.Orders.Remove(order);
        public async Task SaveChangesAsync()
           => await _context.SaveChangesAsync();
    }
}
