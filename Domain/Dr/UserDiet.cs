using Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Domain.Dr
{
    public class UserDiet:Base
    {
        [DisplayName("کابر")]
        public int UserId { get; set; }
        [DisplayName("رژیم")]
        public int DietId { get; set; }
        public DateTime CreateAt { get; set; }
        public Diet diet { get; set; }
        public MyUser User { get; set; }
        [DisplayName("وضعیت")]
        public  UserDietstatus Status { get; set; }
        public SendDiet sendDiet { get; set; }
        public ICollection<FileList> fileLists { get; set; }
        [DisplayName("کد تراکنش درگاه")]
        public string PaymentAuthority { get; set; } // Authority برگشتی از درگاه

        [DisplayName("کد پیگیری بانک")]
        public string PaymentRefId { get; set; } // RefId برگشتی بعد از Verification

        [DisplayName("مبلغ پرداختی")]
        public decimal Amount { get; set; }

        [DisplayName("تاریخ پرداخت")]
        public DateTime? PaymentDate { get; set; }
        public int ParentId { get; set; }
        public UserDiet Parent { get; set; }
        public ICollection<UserDiet> Children { get; set; }
        public ICollection<UserAnswer> userAnswers{ get; set; }


    }
}
