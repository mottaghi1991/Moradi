using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shop
{
    public class PostPrice:Base
    {
        [DisplayName("استان")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public int ProvicesId { get; set; }
        [ForeignKey("ProvicesId")]
        public Province province { get; set; }
        [DisplayName("وزن")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public int Weight { get; set; }
        [DisplayName("قیمت")]
        [Required(ErrorMessage = "وارد کردن {0} اجباری می باشد")]
        public int Price { get; set; }

    }
}
