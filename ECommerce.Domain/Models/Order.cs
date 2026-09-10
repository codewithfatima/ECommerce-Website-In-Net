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
        public DateOnly OrderDate { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal TotalAmount { get; set; }

        public int CustomerId { get; set; } //Fk
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); //lsite of orders


    }
}
