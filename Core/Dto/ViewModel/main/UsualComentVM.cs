using Core.Tools.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.main
{
    public class UsualComentVM
    {
        [DisplayName("نام و نام خانوادگی")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string Name { get; set; }
        [DisplayName("موبایل")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        [IranianPhoneNumber]
        public string Mobile { get; set; }
        [DisplayName("متن")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string Text { get; set; }
        public string status { get; set; }
    }
}
