using Core.Service.Interface.Shop;
using Data.MasterInterface;
using Domain.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Shop
{
    public class ProvinceServices : IProvince
    {
        private readonly IMaster<Province> _master;
        private readonly IMaster<City> _masterCity;
        private readonly IMaster<PostPrice> _masterPrice;

        public ProvinceServices(IMaster<Province> master, IMaster<City> masterCity, IMaster<PostPrice> masterPrice)
        {
            _master = master;
            _masterCity = masterCity;
            _masterPrice = masterPrice;
        }

        public async Task<IEnumerable<Province>> GetAll()
        {
          return await _master.GetAllEfAsync();
        }

        public async Task<IEnumerable<City>> GetAllCityByProId(int ProId)
        {
            return await _masterCity.GetAllEfAsync(a => a.ProvinceId == ProId);
        }

        public async Task<int> PriceValue(int provinceId, decimal Weight)
        {
            int price = 0;

            // گرفتن قیمت‌ها برای استان
            var basePrices = await _masterPrice.GetAllEfAsync(a => a.ProvicesId == provinceId);
            var price2kg = basePrices.FirstOrDefault(a => a.Weight == 2000)?.Price ?? 0;
            var price5kg = basePrices.FirstOrDefault(a => a.Weight == 5000)?.Price ?? 0;

            // حلقه تا وزن تمام بشه
            while (Weight > 0)
            {
                if (Weight >= 5000)
                {
                    // کم کردن یک بسته ۵ کیلو
                    price += price5kg;
                    Weight -= 5000;
                }
                else if (Weight > 2000)
                {
                    // اگر باقی مانده بین ۲ و ۵ کیلو باشه، یه ۵ حساب بشه
                    price += price5kg;
                    Weight = 0; // پایان محاسبه
                }
                else
                {
                    // اگر ≤ ۲ کیلو باشه، قیمت ۲ کیلو حساب بشه
                    price += price2kg;
                    Weight = 0; // پایان محاسبه
                }
            }

            return price;
        }
    }
}
