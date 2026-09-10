using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Models
{
    public class Review
    {
        public int Id { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public int ProductId { get; set; }


        public DateTime Date { get; set; }

        public int StarRating { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool Purchased { get; set; } 
    }

}
 