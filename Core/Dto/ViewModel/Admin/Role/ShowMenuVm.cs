using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Admin.Role
{
    public class ShowMenuVm
    {
        public int PermissionListId { get; set; }

        public int? ParentId { get; set; }
        public string MenuDesc { get; set; }

    }
}
