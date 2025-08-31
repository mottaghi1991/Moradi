using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Users;

namespace Domain.User.Permission
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string RoleTitle { get; set; }
        public List<UserRole> userRoles { get; set; }
        public List<RolePermission> RolePermissions { get; set; }

    }
}
