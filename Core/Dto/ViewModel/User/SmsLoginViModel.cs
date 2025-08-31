using Core.Tools.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.User.Login
{
    public class SmsLoginViModel
    {
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        [Display(Name = "کاربر سیستم")]
        [MaxLength(11, ErrorMessage = "بیشتر مقدار{0}می باشد")]
        [IranianPhoneNumber]
        public string PhoneNumber { get; set; }
    }
    public class AcceptCodeViewModel
    {
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        [Display(Name = "کد تایید")]
        public string SendCode { get; set; }
        public string ReturnUrl { get; set; }
    }
}
