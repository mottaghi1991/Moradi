using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Dr.DietVM
{
    internal class MyDietListVm
    {
        public int DietId { get; set; }
        public string DietName { get; set; }
        public bool IsAnother { get; set; }
        public DateTime LastOrderDate { get; set; }
        public int OrdersCount { get; set; }
    }
}
