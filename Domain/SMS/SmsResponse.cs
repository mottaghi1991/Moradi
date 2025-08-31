using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SMS
{
    public class SmsResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public SmsResponseData Data { get; set; }
    }

    public class SmsResponseData
    {
        public long MessageId { get; set; }
        public decimal Cost { get; set; }
    }
}

