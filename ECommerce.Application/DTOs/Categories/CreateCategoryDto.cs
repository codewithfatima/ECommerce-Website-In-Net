using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Categories
{
    public class CreateCategoryDto
    {
         public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
