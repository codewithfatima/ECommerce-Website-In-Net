using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }   //pK
        public int OrderId { get; set; }  //fk 
        public int ProductId { get; set; } //MANY side → foreign key (product)
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public Product? Product { get; set; }   

    }
}
