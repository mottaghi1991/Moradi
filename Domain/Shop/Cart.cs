using Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shop
{
    public class Cart : Base
    {


        public int? UserId { get; set; }
        public MyUser User { get; set; }
        public virtual ICollection<CartItem> Items { get; set; }

    }
}
