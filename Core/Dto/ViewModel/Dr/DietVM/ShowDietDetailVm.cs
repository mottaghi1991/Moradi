using Domain.Dr;
using Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Dr.DietVm
{
    public class ShowDietDetailVm
    {
        public Diet diet{ get; set; }
        public IEnumerable<Comment> comments { get; set; }

    }
}
