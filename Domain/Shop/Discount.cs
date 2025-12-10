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
    public class Discount:Base
    {
        [DisplayName("کد")]
        [MaxLength(6, ErrorMessage = "طول رشته بیشتر از 50 کاراکتر می باشد")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string Code { get; set; }
        [DisplayName("درصد")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public int Percent { get; set; }
        [DisplayName("شماره سفارش")]
        public int? OrderId { get; set; }
        [DisplayName("استفاده شده")]
        public bool IsUsed{ get; set; }
        [DisplayName("تاریخ مصرف")]
        public DateTime UsedTime { get; set; }

        [ForeignKey("OrderId")]
        public Order Order{ get; set; }
    }
}
