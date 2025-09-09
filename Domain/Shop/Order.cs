using Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shop
{
    public class Order:Base
    {
        public int UserId { get; set; }
        public MyUser User { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

       public virtual ICollection<OrderItem> OrderItems { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
    }
}
