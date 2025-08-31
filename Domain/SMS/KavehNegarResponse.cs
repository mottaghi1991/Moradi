using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SMS
{
    public class KavehNegarResponse
    {
        public long messageId { get; set; }
        public string message { get; set; }
        public int status { get; set; }
        public string statusText { get; set; }
        public string receptor { get; set; }
        public DateTime date { get; set; }
        public int costt{ get; set; }

    }
}
