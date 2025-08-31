using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;


namespace Domain.Dr
{
    public class BMI
    {
        [DisplayName("وزن")]
        [Range(0, 250, ErrorMessage = "عدد باید بین 0 تا 250 باشد")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public int Weight { get; set; }
        [DisplayName("قد")]
        [Range(0, 250, ErrorMessage = "عدد باید بین 0 تا 250 باشد")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public int Height { get; set; }
        [DisplayName("سن")]
        [Range(18, 100, ErrorMessage = "عدد وارده بین 18 تا 100 باشد")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public int Age { get; set; }
        [DisplayName("استخوان بندی")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public Stone Stone { get; set; }
        [DisplayName("جنسیت")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public Gender Gender { get; set; }

    }
}
