using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Payment.PaymentFirstResponse;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Payment
{
    public class PaymentFinalResponse
    {
        public Data data { get; set; }
        public Errors Error { get; set; }
        public class Data
        {
            public int code { get; set; }
            public string Message { get; set; }
            public string card_hash { get; set; }
            public string card_pan { get; set; }
            public string ref_id { get; set; }
            public string fee_type { get; set; }
            public int fee { get; set; }
        }
        public class Errors
        {
            public string message { get; set; }
            public int code { get; set; }
        }
    }
  
}
