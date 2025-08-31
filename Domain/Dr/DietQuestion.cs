using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dr
{
    [Table("DietQuestions")]
    public class DietQuestion
    {

        [DisplayName("رژیم")]
        public int DietId { get; set; }
        [DisplayName("سوال")]
        public int QuestionId { get; set; }
        public Diet Diet { get; set; }
        public Question Question { get; set; }
    }
}
