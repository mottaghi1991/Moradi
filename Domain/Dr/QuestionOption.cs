using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dr
{
    public class QuestionOption:Base
    {
        [DisplayName("سوال")]
        public int QuestionId { get; set; }
        [DisplayName("گزینه سوال")]
        public string OptionText { get; set; } // مثلا: مجرد، متاهل
        public Question Question { get; set; }
    }
}
