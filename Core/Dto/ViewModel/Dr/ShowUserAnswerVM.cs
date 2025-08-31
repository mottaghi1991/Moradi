using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Dr
{
    public class ShowUserAnswerVM
    {
        public int Id { get; set; }
        [DisplayName("متن سوال")]
        public string QuestionText { get; set; }
        public int DietId { get; set; }
        public int UserDietRecordId { get; set; }
        [DisplayName("پاسخ کاربر")]
        public string Answer { get; set; }
    }
}
