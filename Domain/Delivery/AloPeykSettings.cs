using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Delivery
{
    public class AloPeykSettings
    {
        public string ApiBaseUrl { get; set; }
        public string Token { get; set; }
        public AloPeykOriginSettings Origin { get; set; }
    }
    public class AloPeykOriginSettings
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
    }
}
