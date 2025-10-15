using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Delivery
{
    public class AloPeykWebhookOrder
    {
        public int Id { get; set; }
        public string Status { get; set; } // new, searching, accepted, delivering, delivered, finished, cancelled
        public string Updated_At { get; set; }
        public AloPeykWebhookCourier Courier { get; set; }
    }
}
