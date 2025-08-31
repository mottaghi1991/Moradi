
using Domain.DrShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Shop
{
    public interface ICategory
    {
        Task<IEnumerable<Category>> GetAllByActive(bool active);
    }
}
