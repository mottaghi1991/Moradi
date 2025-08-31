using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Dr.DietVM
{
    public class EditDietVm
    {
        public int Id { get; set; }
        [DisplayName("نام رژیم")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        [MaxLength(50, ErrorMessage = "طول رشته بیشتر از 50 کاراکتر می باشد")]
        public string Name { get; set; }
        [DisplayName("قیمت(ریال)")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public int Price { get; set; }
        [DisplayName("توضیحات")]
        public string Descript { get; set; }
        [DisplayName("توضیحات بیشتر")]
        public string SpecialDescript { get; set; }
        [DisplayName("ویدئو")]
        public string Video { get; set; }
        [DisplayName("فعال")]
        public bool Status { get; set; }
        // سایر فیلدهایی که نیاز داری
        [DisplayName("تصویر")]
    
        public IFormFile ImageFile { get; set; }
        [DisplayName("تصویر")]
        public string Image { get; set; }
    }
}
