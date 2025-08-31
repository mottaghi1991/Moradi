using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Dr
{
    public class BlogPageVm
    {
        public IEnumerable<Post> Post{ get; set; }
        public int TotalPage { get; set; }
        public int Page { get; set; }
    }
}
