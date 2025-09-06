using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.main
{
    public class Paging<T>  where T :class 
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T>bjects { get; set; }
        public string fullName { get; set; }
        public string mobile { get; set; }
        public string paymentStatus { get; set; }
        public int? userId { get; set; }
    }
}
