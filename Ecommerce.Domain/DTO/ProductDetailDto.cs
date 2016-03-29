namespace Ecommerce.Domain.DTO
{
    public class ProductDetailDto
    {
        public int ProductId { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}