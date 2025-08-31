using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Base
    {
        public int Id { get; set; }
        [DisplayName("حذف شده")]
        public Boolean IsDeleted { get; set; }
        [DisplayName("تاریخ حذف")]
        public DateTime? DeleteTime { get; set; }=DateTime.Now;
    }
}
