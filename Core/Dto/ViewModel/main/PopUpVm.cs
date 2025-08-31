using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.main
{
    public class PopUpVm
    {
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string Title { get; set; }
        [DisplayName("توضیحات")]
        public string Descript { get; set; }
        [DisplayName("تصویر")]
        public string Image { get; set; }
        [DisplayName("وضعیت")]
        public bool IsActive { get; set; }=false;
        public IFormFile ImageFile{ get; set; }
    }
}
