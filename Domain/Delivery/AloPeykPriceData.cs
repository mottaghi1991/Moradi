using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Delivery
{
    public class AloPeykPriceObject
    {
        public List<AloPeykPriceAddress> Addresses { get; set; }

        public double Price { get; set; }
        public bool Credit { get; set; }
        public int Distance { get; set; }
        public int Duration { get; set; }

        public string Status { get; set; }        // "OK"
        public double User_Credit { get; set; }
        public double Price_With_Return { get; set; }
        public double Final_Price { get; set; }

        public double Discount { get; set; }
        public string City { get; set; }
        public string City_Fa { get; set; }
        public string Transport_Type { get; set; }

        public bool Has_Return { get; set; }
        public bool Cashed { get; set; }
        public bool Scheduled { get; set; }
    }
    public class AloPeykPriceAddress
    {
        public string Type { get; set; }     // "origin" یا "destination"
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string City_Fa { get; set; }
        public int Priority { get; set; }

        public double? Distance { get; set; }
        public double? Duration { get; set; }
        public double? Coefficient { get; set; }
        public double? Price { get; set; }
    }

}
