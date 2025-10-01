using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shop
{
    public class City:Base
    {
        public string Title { get; set; }

        public int ProvinceId { get; set; }
        [ForeignKey("ProvinceId")]
        public Province province { get; set; }
    }
}
