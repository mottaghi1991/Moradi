using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PersonalData
{
    public  class Setting:Base
    {
        [DisplayName("نام و نام خانوادگی")] 
        [MaxLength(50, ErrorMessage = "بیشتر مقدار{0}می باشد")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public   string Name { get; set; }
        [DisplayName("شغل")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        [MaxLength(50, ErrorMessage = "بیشتر مقدار{0}می باشد")]
        public   string jobs{ get; set; }
        [DisplayName("ایمیل")]
        [MaxLength(30, ErrorMessage = "بیشتر مقدار{0}می باشد")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        [DataType(dataType:DataType.EmailAddress,ErrorMessage = "فرمت وارد شده صحیح نمی باشد .")]
        public   string Email { get; set; }
        [DisplayName("تلفن")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        [DataType(dataType: DataType.PhoneNumber, ErrorMessage = "فرمت وارد شده صحیح نمی باشد .")]
        [MaxLength(50, ErrorMessage = "بیشتر مقدار{0}می باشد")]
        public   string Phone { get; set; }

        [DisplayName("تاریخ تولد")]
        [MaxLength(50, ErrorMessage = "بیشتر مقدار{0}می باشد")]
        public DateTime Birthday { get; set; }
        [DisplayName("مکان")]
        [MaxLength(50, ErrorMessage = "بیشتر مقدار{0}می باشد")]
        public  string Location { get; set; }
        [DisplayName("درباره من")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public  string Aboute { get; set; }
        [DisplayName("توئیتر")]
        [MaxLength(50, ErrorMessage = "بیشتر مقدار{0}می باشد")]
        public  string Tweeter { get; set; }
        [DisplayName("لینکدین")]
        [MaxLength(50, ErrorMessage = "بیشتر مقدار{0}می باشد")] 
        public string Linkedin { get; set; }
        [DisplayName("اینستاگرام")]
        public  string Instagram{ get; set; }
        [DisplayName("تصویر پس زمینه")]
        [MaxLength(50, ErrorMessage = "بیشتر مقدار{0}می باشد")]
        public  string BackgroundImage { get; set; }
        [DisplayName("تصویر پروفایل")]
        public string ProfileImage { get; set; }

    }
}
