using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Delivery
{
    public class AloPeykStatusWebhookModel
    {
        public string order_id { get; set; }
        public string status { get; set; }
        public string tracking_code { get; set; }
        public Courier courier { get; set; }

        public class Courier
        {
            public string name { get; set; }
            public string phone { get; set; }
        }
    }
}
