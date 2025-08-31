using Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
  public  class UserGroup
    {
        [Key]
        public int UserGroupId { get; set; }
        public string GroupName { get; set; }
       
        public List<MyUser> User { get; set; }
    }
}
