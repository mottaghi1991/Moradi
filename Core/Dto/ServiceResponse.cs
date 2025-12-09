using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class ServiceResponse
    {
        public int ErrorId { get; set; } = -1;
        public string? ErrorTitle { get; set; }
        public object  Object{ get; set; }
    }
}
