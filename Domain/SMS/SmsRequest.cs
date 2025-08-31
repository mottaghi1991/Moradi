using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SMS
{
    /// <summary>
    /// براس سایت sms.ir
    /// </summary>
    public class SmsRequest
    {
        public string Mobile { get; set; }
        public int TemplateId { get; set; }
        public List<patameter> smsParameters { get; set; }
    }
    public class patameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
