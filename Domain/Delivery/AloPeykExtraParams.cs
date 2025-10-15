using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Delivery
{
    public class AloPeykExtraParams
    {
        public int InternalOrderId { get; set; }
        public string PaymentReference { get; set; } // مثلاً ZP_12345 از زرین‌پال
    }
}
