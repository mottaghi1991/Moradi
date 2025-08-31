using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Dr.QuestionFolder
{
    public class QuestionWithUserAnswerVm
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string UserAnswer { get; set; }
    }
}
