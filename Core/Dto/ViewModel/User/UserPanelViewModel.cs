using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.User
{
    public class InformationUserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Wallet { get; set; }
    }

    public class UserPanelViewModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
