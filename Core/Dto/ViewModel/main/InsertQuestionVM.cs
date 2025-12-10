using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.main
{
    public class InsertQuestionVM
    {
        public int CommentId { get; set; }
        [DisplayName("سوال")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string Question { get; set; }
        [DisplayName("پاسخ")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string Answer { get; set; }
    }
}
