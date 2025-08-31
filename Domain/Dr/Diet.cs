using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dr
{
    public class Diet:Base
    {
        [DisplayName("نام رژیم")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        [MaxLength(50, ErrorMessage = "طول رشته بیشتر از 50 کاراکتر می باشد")]
        public string Name { get; set; }
        [DisplayName("تصویر")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string Image { get; set; }
        [DisplayName("قیمت")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public int Price { get; set; }
        public bool Status { get; set; }
        public string Descript { get; set; }
        public string SpecialDescript { get; set; }
        public string Video { get; set; }
        public  ICollection<Question> Questions { get; set; }
        public ICollection<DietQuestion> DietQuestions { get; set; }
        public ICollection<UserDiet> userDiets{ get; set; }
        public ICollection<UserAnswer> userAnswers { get; set; }

    }
}
