using Domain.Dr;
using Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.SettingDto
{
    public class ShowFooterVM
    {
        public Domain.Main.Setting setting{ get; set; }
        public IEnumerable<Diet> Diet{ get; set; }
    }
}
