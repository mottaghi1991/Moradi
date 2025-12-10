using Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.main
{
    public class ShowContactUsVM
    {
        public Setting Setting { get; set; }
        public IEnumerable<Comment>comments{ get; set; }
    }
}
