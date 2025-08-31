using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Dr.QuestionFolder
{
    public class DietQuestionVm
    {
        public int DietId { get; set; }
        public string DietName { get; set; }
        public bool FirstForm { get; set; }
        public IEnumerable<DietQuestion> DietQuestion { get; set; }
    }
}
