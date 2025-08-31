using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dr
{
    public class Post:Base
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Summry { get; set; }
        public string Descript { get; set; }
    }
}
