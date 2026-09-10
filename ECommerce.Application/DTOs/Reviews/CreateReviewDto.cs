using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Reviews
{
    public class CreateReviewDto
    {
      public string CustomerName    { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int StarRating { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
