using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Seo
{
    public class SeoData:Base
    {

        // 📌 شناسه یا کلید صفحه (مثلاً Slug یا PageType)
        [StringLength(100)]
        [DisplayName("صفحه")]
        public string EntityType { get; set; }  // مثلاً "home", "about", "blog/seo-intro"

        // ✅ عنوان متا
        [DisplayName("عنوان متا")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string MetaTitle { get; set; }


        [DisplayName("توضیحات متا")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string MetaDescription { get; set; }

        [DisplayName("کلمات کلیدی")]
        public string MetaKeywords { get; set; }

        [DisplayName("آدرس canoncial")]
        [Url(ErrorMessage ="مقدار وارد شده صحیح نمی باشد.")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string CanonicalUrl { get; set; }

        [DisplayName("عنوان برای شبکه اجتماعی")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string OgTitle { get; set; }

        [DisplayName("توضیح کوتاه برای شبکه اجتماعی")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string OgDescription { get; set; }
        [DisplayName("تصویر شاخص")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string OgImage { get; set; }
        [DisplayName("نوع صفحه")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string OgType { get; set; }

        [DisplayName("ساختار schema")]
    
        public string JsonLdSchema { get; set; }
        [DisplayName("نوع انتشار")]
        public bool NoIndex { get; set; }

  
    }
}
