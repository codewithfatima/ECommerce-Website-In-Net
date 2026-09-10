using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }  
        public string Name { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new List<Product>();

    }
}
