using Domain.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Shop.Address
{
    public class GetAddressOfUserVM
    {
        public IEnumerable<ShippingAddres> OldAddress{ get; set; }
        public ShippingAddres NewAddress { get; set; }
        public bool ShowNewAddressForm { get; set; }
    }
}
