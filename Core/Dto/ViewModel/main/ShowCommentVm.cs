using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.main
{
    public class ShowCommentVm
    {
        public int Id { get; set; }
        public int DietId { get; set; }
        public bool IsApproved { get; set; }
        public EntityType EntityType { get; set; }
        public string Mobile { get; set; }
        [DisplayName("پیام کاربر")]
        public string UserComment { get; set; }
        [DisplayName("پاسخ ادمین")]
        public string AdminComment { get; set; }
        public string CreateDate { get; set; }
    }
}
