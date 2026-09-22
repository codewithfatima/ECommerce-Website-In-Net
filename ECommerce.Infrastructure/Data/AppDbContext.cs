using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Infrastructure.Data;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ECommerce.Domain.Models;

namespace ECommerce.Infrastructure.Data
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
       public AppDbContext(DbContextOptions<AppDbContext> options) :base (options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Whislist> Whislists { get; set; }

    }
}
