using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Delivery
{
    public class OrderStatus
    {
        public string OrderId { get; set; }
        public string Status { get; set; } // pending, delivering, delivered
        public DateTime LastUpdate { get; set; }
    }
}
