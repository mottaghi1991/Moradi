using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.User
{
    public class FillFromVm
    {
        [DisplayName("نام و نام خانوادگی")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string FullName { get; set; }
        [DisplayName("شغل")]
        public string Job { get; set; }
        [DisplayName("جنسیت")]
        public Gender gender{ get; set; }
        [DisplayName("شهر")]
        public string City { get; set; }
    }
}

