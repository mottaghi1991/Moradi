using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Shop.Address
{
    public class AddAdressVm
    {
        [DisplayName("استان")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]


        public int? provinceId { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        [DisplayName("آدرس")]
        public string AddressLine { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        [DisplayName("کد پستی")]
        public string PostalCode { get; set; }
    }
}
