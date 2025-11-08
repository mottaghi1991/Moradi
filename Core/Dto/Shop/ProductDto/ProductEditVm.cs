using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Store.ProductDto
{
    public class ProductEditVm
    {
        public int Id { get; set; }
        [DisplayName("نام محصول")]
        [MaxLength(100, ErrorMessage = "طول نام محصول بیشتر از 100 کاراکتر است")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        public string ProductName { get; set; }


        [DisplayName("ویژگی ها")]
        public string Attrib { get; set; }

        [DisplayName("تصویر محصول")]
        public string ImageUrl { get; set; }
        [DisplayName("وزن محصول گرم")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        public decimal Weight { get; set; }
        public IFormFile ImageFile { get; set; }
 
        [DisplayName("دسته بندی")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public int CategoryId { get; set; }
        [DisplayName("وضعیت")]
        public bool IsDeleted { get; set; }
        [DisplayName("ویدئو")]
        public string? Video { get; set; }
    }
}
