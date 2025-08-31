using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Dr
{
    public class ShowUserFormVM
    {
        public int UserId { get; set; }
        public IEnumerable<UserDiet> userDiets { get; set; }
        public IEnumerable<ShowUserAnswerVM> showUserAnswerVMs { get; set; }
        public IEnumerable<FileList> UserFile { get; set; }
    }
}
