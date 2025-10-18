using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Shop.Order
{
    public class OrderSendPostVm
    {
        public int Id { get; set; }
        [DisplayName("تاریخ ارسال")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string? SendDate { get; set; }
        [DisplayName("شناسه")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string? PostIdentity { get; set; }
    }
}
