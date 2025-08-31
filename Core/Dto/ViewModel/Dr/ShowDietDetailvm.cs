using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Dr
{
    public class ShowPostDetailvm
    {
        public Post post{ get; set; }
        public IEnumerable<Post> TopPost { get; set; }
    }
}
