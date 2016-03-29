using System.Collections.Generic;

namespace Ecommerce.Domain.DTO
{
    public class OrderDto
    {
        public IEnumerable<ProductDetailDto> Details { get; set; } 
    }
}