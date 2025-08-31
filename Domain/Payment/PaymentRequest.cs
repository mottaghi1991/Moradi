using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Payment
{
    public class PaymentRequest
    {
        public string merchant_id { get; set; }
        public int amount { get; set; }
        public string callback_url { get; set; }
        public string description { get; set; }
        public Metadata Metadata { get; set; }
    }

    public class Metadata
    {
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}
