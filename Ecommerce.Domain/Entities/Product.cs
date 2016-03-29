using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Domain.Entities
{
    public class Product
    {

        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal ActualCost { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
