using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }   //pK
        public OrderStatus Status { get; set; }     
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

        public string UserId { get; set; } = string.Empty;  //FK to ApplicationUser
        public ApplicationUser? User { get; set; }
        public List<OrderItem> Items { get; set; } = new();

    }
}
