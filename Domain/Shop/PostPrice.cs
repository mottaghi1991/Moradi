using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shop
{
    public class PostPrice:Base
    {
        public int ProvicesId { get; set; }
        [ForeignKey("ProvicesId")]
        public Province province { get; set; }
        public int Weight { get; set; }
        public int Price { get; set; }

    }
}
