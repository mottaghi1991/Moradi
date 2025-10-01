using Domain.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Shop.Address
{
    public class FinalInvoiceVM
    {
        public ShippingAddres Address { get; set; }
        public IEnumerable<CartItem> Items{ get; set; }
        public int SendPrice { get; set; }

    }
}
