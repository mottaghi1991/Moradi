using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Payment
{
    public class PaymentFirstResponse
    {
            public Data data { get; set; }
            public Errors Error { get; set; }
        
        public class Data
        {
            public string authority { get; set; }
            public int fee { get; set; }
            public string fee_type { get; set; }
            public int code { get; set; }
            public string message { get; set; }

        }
        public class Errors
        {
            public string message { get; set; }
            public int code { get; set; }
        }

    }
}

