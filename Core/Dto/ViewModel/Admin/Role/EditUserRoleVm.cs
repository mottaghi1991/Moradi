using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.User;

namespace Core.Dto.ViewModel.Admin.Role
{
    public class EditUserRoleVm
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
