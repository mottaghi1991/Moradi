using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dr
{
    public class Question:Base
    {
        [DisplayName("عنوان")]
        [MaxLength(500, ErrorMessage = "طول رشته بیشتر از 500 کاراکتر می باشد")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public string Name { get; set; }
        [DisplayName("ترتیب")]
        public decimal Order { get; set; }
        public FieldType FieldType   { get; set; }
        [DisplayName("ضروری")]
        public bool IsRequired { get; set; }
        [DisplayName("مربوط به فرم اولیه")]
        public bool FirstForm { get; set; }
        public  ICollection<Diet> Diets { get; set; }
        public ICollection<DietQuestion> DietQuestions { get; set; }
        public ICollection<UserAnswer> userAnswers { get; set; }
        public ICollection<QuestionOption> Options { get; set; }

    }
}
