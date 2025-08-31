using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.User.Permission
{
    public class RolePermission
    {
        [Key] 
        public int RP_Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionListId { get; set; }
  
        public Role Role { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public PermissionList PermissionList { get; set; }

    }



}
