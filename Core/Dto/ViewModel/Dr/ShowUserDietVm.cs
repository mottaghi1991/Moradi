using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Dr
{
    public class ShowUserDietVm
    {
        public SendDiet SendDiet { get; set; }
        public IEnumerable<FileList> fileLists { get; set; }
    }
}
