using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shop
{
    public class Province:Base
    {
        [DisplayName("استان")]
        public string Title { get; set; }
   
        public virtual ICollection<ShippingAddres> ShippingAddres { get; set; }
        public virtual ICollection<PostPrice> PostPrices{ get; set; }
    }
}
