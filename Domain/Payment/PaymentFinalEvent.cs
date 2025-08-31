
using Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Payment
{
    public class PaymentFinalEvent
    {
        [Key]
        public int Id { get; set; }
        public int code { get; set; }
        [DisplayName("مبلغ")]
        public int Price { get; set; }
        public int ExamId { get; set; }
        public string Message { get; set; }
        public string? card_hash { get; set; }
        public string? card_pan { get; set; }
        [DisplayName("کد رهگیری")]
        public string? ref_id { get; set; }
        public string? fee_type { get; set; }
        [DisplayName("کارمزد")]
        public int? fee { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public MyUser myUser{ get; set; }
      
      
        public DateTime InsertDate { get; set; } = DateTime.Now;
    }
}
