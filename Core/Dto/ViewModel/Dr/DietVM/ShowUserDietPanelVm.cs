using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Dr.DietVM
{
    public class ShowUserDietPanelVm
    {
        public int UserdietId { get; set; }
        [DisplayName("نام کاربر")]
        public string UserPanelName { get; set; }
        [DisplayName("رژیم گیرنده")]

        public string UserDiteName { get; set; }
        [DisplayName("تاریخ ثبت")]

        public DateTime CreateAt { get; set; }
        [DisplayName("وضعیت پرداخت")]

        public UserDietstatus status { get; set; }
        [DisplayName("نام رژیم")]

        public string DietName { get; set; }
        public int DietId { get; set; }
        public int ParentId { get; set; }
    }
}
