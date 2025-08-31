using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SMS
{
    public class UserOtp:Base
    {
        [DisplayName("شماره موبایل")]
        [MaxLength(11, ErrorMessage = "طول رشته بیشتر از 50 کاراکتر می باشد")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string PhoneNumber { get; set; }
        [DisplayName("کد فعال سازی")]
        [MaxLength(6, ErrorMessage = "طول رشته بیشتر از 50 کاراکتر می باشد")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string Code { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime ExpireAt { get; set; }
        public bool IsUsed{ get; set; }
    }
}
