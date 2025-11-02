using Domain.Shop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Shop.Batch
{
    public class BtachDetailListVm
    {
        public int Id { get; set; }
        [DisplayName("قیمت(ریال)")]
        [Range(0, double.MaxValue, ErrorMessage = "قیمت نمی‌تواند منفی باشد")]
        public decimal Price { get; set; }

        [DisplayName("موجودی")]
        public int Stock { get; set; }
        [DisplayName("درصد تخفیف")]
        public int OffPricePercent { get; set; }
        [DisplayName("وضعیت")]
        public bool IsActive { get; set; }

        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        [DisplayName("تاریخ ورود")]
        public DateTime CreateDate { get; set; }
        [DisplayName("باقی مانده")]
        public int Remain { get; set; }
        [DisplayName("فروخته شده")]
        public int Sold { get; set; }
    }
}
