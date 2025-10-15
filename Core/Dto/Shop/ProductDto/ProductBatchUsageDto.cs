using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Shop.ProductDto
{
    public class ProductBatchUsageDto
    {
        public int ProductBatchId { get; set; }
        public int InitialStock { get; set; }   // کل تعداد اولیه ثبت‌شده هنگام ورود کالا
        public int SoldCount { get; set; }      // تعداد استفاده‌شده در سفارش‌ها
        public int RemainingCount { get; set; } // موجودی باقی‌مانده (محاسبه‌شده)
    }
}
