using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Dto.ViewModel.Store.ProductDto
{
    public class ProductAddVM
    {
        [DisplayName("نام محصول")]
        [MaxLength(100, ErrorMessage = "طول نام محصول بیشتر از 100 کاراکتر است")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        public string ProductName { get; set; }

        [DisplayName("توضیحات")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string Description { get; set; }

        [DisplayName("قیمت")]
        [Range(0, double.MaxValue, ErrorMessage = "قیمت نمی‌تواند منفی باشد")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public decimal Price { get; set; }

        [DisplayName("موجودی")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public int Stock { get; set; }
        [DisplayName("درصد تخفیف")]

        public int OffPricePercent { get; set; } = 0;

        [DisplayName("ویژگی ها")]
        public string Attrib { get; set; }
        [DisplayName("مشخصات")]
        public string Proper { get; set; }
        [DisplayName("تصویر محصول")]
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
     
        public IFormFile ImageFile{ get; set; }
        [DisplayName("وضعیت")]
        public bool IsActive { get; set; }
        [DisplayName("دسته بندی")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public int SubCategoryId { get; set; }
    }
}
