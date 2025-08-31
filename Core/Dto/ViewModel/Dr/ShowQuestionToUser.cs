using Core.Dto.ViewModel.Dr.QuestionFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Dr
{
    public class ShowQuestionToUserVM
    {
        public int DietId { get; set; }
        public string DietName { get; set; }
        public int? parentId { get; set; }
        public List<DynamicQuestionVm> Questions { get; set; }
    }
}
