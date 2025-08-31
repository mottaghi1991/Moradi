using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Store
{
    public class ProductImage:Base
    {
        public int ProductId { get; set; }
        [DisplayName("تصویر")]
        public string ImageUrl { get; set; }
        [ForeignKey("ProductId")]
        public Product product { get; set; }
    }
}
