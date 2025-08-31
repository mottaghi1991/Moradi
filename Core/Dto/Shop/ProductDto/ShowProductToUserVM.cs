using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Store.ProductDto
{
    public class ShowProductListToUserVM
    {
        public int Id { get; set; }
        [DisplayName("نام محصول")]
        [MaxLength(100, ErrorMessage = "طول نام محصول بیشتر از 100 کاراکتر است")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        public string ProductName { get; set; }
        [DisplayName("توضیحات")]
        public string Description { get; set; }
        [DisplayName("قیمت")]
        [Range(0, double.MaxValue, ErrorMessage = "قیمت نمی‌تواند منفی باشد")]
        public decimal Price { get; set; }
        [DisplayName("درصد تخفیف")]
        public int OffPricePercent { get; set; }
        [DisplayName("تصویر محصول")]
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
        public string FeatureValueName { get; set; }
    }
}
