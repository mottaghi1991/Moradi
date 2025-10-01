using Domain.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Shop
{
    public interface IProvince
    {
        Task<IEnumerable<Province>> GetAll();
        Task<IEnumerable<City>> GetAllCityByProId(int ProId);
        Task<int> PriceValue(int provinceId, decimal Weight);
    }
}
