
using Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Payment
{
    public class PaymentEventDatabase
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Price { get; set; }

        public int ExamId { get; set; }
        public string? authority { get; set; }
        public int? fee { get; set; }
        public string? fee_type { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public DateTime InsertDate { get; set; } = DateTime.Now;
        [ForeignKey("UserId")]
        public MyUser myUser{ get; set; }
   
    }
}
