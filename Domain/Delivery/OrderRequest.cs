using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Delivery
{
    public class OrderRequest
    {
        public string TransportType { get; set; } // motorbike, van, etc.
        public string OriginAddress { get; set; }
        public double OriginLat { get; set; }
        public double OriginLng { get; set; }
        public string DestinationAddress { get; set; }
        public double DestinationLat { get; set; }
        public double DestinationLng { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string Description { get; set; }
        public bool PayAtOrigin { get; set; }
    }
}
