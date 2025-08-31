using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Domain.User.Permission;

namespace Core.Dto.ViewModel.Admin.Role
{
    public class EditPermissionRoleVm
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public IEnumerable<RolePermission> MenuVm { get; set; }
    }
}
