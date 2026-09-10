namespace ECommerce.Web.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public decimal Price { get; set; }
        public string Thumbnail { get; set; } = "";
    }

}
