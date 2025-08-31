using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dr
{
    public class SendDiet:Base
    {
        [DisplayName("توضیحات")]
        public string Descript { get; set; }
       
        public int UserDietId { get; set; }
        public UserDiet  userDiet{ get; set; }
    }
}
