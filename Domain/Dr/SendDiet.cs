using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dr
{
    public class SendDiet:Base
    {
        [DisplayName("توضیحات")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string Descript { get; set; }
       
        public int UserDietId { get; set; }
        public UserDiet  userDiet{ get; set; }
    }
}
