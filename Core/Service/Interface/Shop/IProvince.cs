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
        Task<IEnumerable<PostPrice>> GetAllPrice();
        Task<IEnumerable<PostPrice>> GetAllPriceByProvicedid(int provicedid);
        Task<bool> InsertPrice(PostPrice postPrice);
        Task<bool> UpdatePrice(PostPrice postPrice);
        Task<bool> DeletePrice(PostPrice postPrice);
        Task<PostPrice> GetPriceById(int postpriceId);
    }
}
