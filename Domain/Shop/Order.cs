using Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shop
{
    public class Order:Base
    {
        public int UserId { get; set; }
        public MyUser User { get; set; }

        public DateTime OrderDate { get; set; }
        [DisplayName("مبلغ پرداختی")]
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        [DisplayName("ضعیت")]
        public int SendPrice { get; set; }
        [DisplayName("کد تراکنش درگاه")]
        public string PaymentAuthority { get; set; } // Authority برگشتی از درگاه

        [DisplayName("کد پیگیری بانک")]
        public string PaymentRefId { get; set; } // RefId برگشتی بعد از Verification

        [DisplayName("مبلغ اجناس")]
        public decimal Amount { get; set; }

        [DisplayName("تاریخ پرداخت")]
        public DateTime? PaymentDate { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public int ShippingAddressId { get; set; }
        [ForeignKey("ShippingAddressId")]
        public ShippingAddres ShippingAddress { get; set; }
    }
}
