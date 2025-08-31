using Core.Service.Interface.Shop;
using Data.MasterInterface;
using Domain.DrShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Shop
{
    public class CategoriesServices : ICategory
    {
        private readonly IMaster<Category> _master;

        public CategoriesServices(IMaster<Category> master)
        {
            _master = master;
        }

        public async Task<IEnumerable<Category>> GetAllByActive(bool active)
        {
            return await _master.GetAllEfAsync(a => a.IsActive == active);
        }
    }
}
