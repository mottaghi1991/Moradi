using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.User
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        [Display(Name = "کاربر سیستم")]
        [MaxLength(50, ErrorMessage = "بیشتر مقدار{0}می باشد")]
        public string UserName { get; set; }


        [Display(Name = "رمزعبور")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string PassWord { get; set; }
        [Compare("PassWord", ErrorMessage = "رمز عبور یکسان نمی باشد")]
        public string RePassword { get; set; }

    }
}
