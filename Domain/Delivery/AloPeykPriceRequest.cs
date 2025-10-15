using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Delivery
{
    public class AloPeykPriceRequest
    {
        public string TransportType { get; set; } = "motorbike"; // نوع وسیله حمل
        public double FromLat { get; set; }
        public double FromLng { get; set; }
        public double ToLat { get; set; }
        public double ToLng { get; set; }
    }
}
