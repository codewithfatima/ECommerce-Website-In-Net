using ECommerce.Application.DTOs.Products;

namespace ECommerce.Web.Models
{
    public class PagedProductResult
    {
        public List<ProductDto> Products { get; set; } 
        public int TotalCount { get; set; }
    }
}
