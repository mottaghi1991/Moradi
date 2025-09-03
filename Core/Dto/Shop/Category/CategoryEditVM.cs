using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Shop.Category
{
    public class CategoryEditVM
    {
        public int Id { get; set; }
        [DisplayName("عنوان")]
        [MaxLength(50, ErrorMessage = "طول رشته بیشتر از 50 کاراکتر می باشد")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string CategoryName { get; set; }

        [DisplayName("تصویر")]
        public string Image { get; set; }
        [DisplayName("تصویر")]
        public IFormFile ImageFile { get; set; }
    }
}
