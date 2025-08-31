using Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Main
{
    public class Comment:Base
    {
        public int? ParentId { get; set; }
        public int? UserId { get; set; }
        public string  Name{ get; set; }
    
        public string Mobile { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string Text { get; set; }
        [DisplayName("بخش")]
        public EntityType EntityType { get; set; }
        public int? EntityId { get; set; }
        [DisplayName("تاریخ ایجاد")]

        public DateTime CreateDate { get; set; }
        [DisplayName("وضعیت")]

        public bool IsApproved { get; set; }
        public MyUser myUser{ get; set; }

    }
}
