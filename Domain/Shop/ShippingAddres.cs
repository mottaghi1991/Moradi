using Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shop
{
    public class ShippingAddres:Base
    {
        [DisplayName("استان")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]

        public int provinceId { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        [DisplayName("آدرس")]
        public string AddressLine { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        [DisplayName("کد پستی")]
        public string PostalCode { get; set; }
        [ForeignKey("provinceId")]
        public Province  province{ get; set; }
        public int UserId { get; set; }
        [DisplayName("عرض جغرافیایی")]
        public double? Latitude { get; set; }   // لازم برای الوپیک

        [DisplayName("طول جغرافیایی")]
        public double? Longitude { get; set; }  // لازم برای الوپیک
        [ForeignKey("UserId")]
        public MyUser  user{ get; set; }

    }
}
