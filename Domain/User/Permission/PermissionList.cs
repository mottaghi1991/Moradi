using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.User.Permission
{
    public class PermissionList
    { 
        [DisplayName("شناسه دسترسی")]
        public int Id { get; set; }
        [DisplayName("ردیف")]
        public int Radif { get; set; }
        [DisplayName("دسته بندی")]
        public string Area { get; set; }
        [DisplayName("کنترلر")]
        public string ControllerName { get; set; }
        [DisplayName("متد")]
        public string ActionName { get; set; }
        [DisplayName("توضیحات")]
        public string Descript { get; set; }
        [DisplayName("والد")]
        public int? ParentId { get; set; }
        [DisplayName("ردیف")]
        public int? Status { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public List<RolePermission> RolePermissions { get; set; }
    }
}
