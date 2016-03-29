using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecommerce.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public double Amount { get; set; }

        public DateTime DateCreated { get; set; }

        public int ConfirmationNumber { get; set; }

        public string UserId { get; set; }

        // Navigation property
        public User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }

}
