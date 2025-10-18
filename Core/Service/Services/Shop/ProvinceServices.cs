using Core.Service.Interface.Shop;
using Data.MasterInterface;
using Domain.Shop;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> DeletePrice(PostPrice postPrice)
        {
            return await _masterPrice.DeleteAsync(postPrice);        }

        public async Task<IEnumerable<Province>> GetAll()
        {
            return await _master.GetAllEfAsync();
        }

        public async Task<IEnumerable<City>> GetAllCityByProId(int ProId)
        {
            return await _masterCity.GetAllEfAsync(a => a.ProvinceId == ProId);
        }

        public async Task<IEnumerable<PostPrice>> GetAllPrice()
        {
            return _masterPrice.GetAllAsQueryable().Include(a => a.province); }

        public async Task<IEnumerable<PostPrice>> GetAllPriceByProvicedid(int provicedid)
        {
            return _masterPrice.GetAllAsQueryable(a => a.ProvicesId == provicedid).Include(a => a.province);    
        }

        public async Task<PostPrice> GetPriceById(int postpriceId)
        {

            var obj= await _masterPrice.GetAllEfAsync(a => a.Id == postpriceId);
            return obj.FirstOrDefault();
        }

        public async Task<bool> InsertPrice(PostPrice postPrice)
        {
            var obj = await _masterPrice.InsertAsync(postPrice);
            return obj != null;
        }

        public async Task<int> PriceValue(int provinceId, decimal weight)
        {
            // گرفتن همه تعرفه‌ها برای استان موردنظر
            var priceList = (await _masterPrice
                .GetAllEfAsync(a => a.ProvicesId == provinceId && !a.IsDeleted))
                .OrderBy(a => a.Weight)
                .ToList();

            if (!priceList.Any())
                return 0; // اگر هیچ قیمتی تعریف نشده

            // اگر وزن کمتر از کمترین مقدار ثبت‌شده است
            if (weight <= priceList.First().Weight)
                return priceList.First().Price;

            // اگر وزن دقیقاً برابر با یکی از مقادیر تعریف‌شده است
            var exactMatch = priceList.FirstOrDefault(x => x.Weight == weight);
            if (exactMatch != null)
                return exactMatch.Price;

            // اگر وزن بین دو مقدار باشد
            for (int i = 0; i < priceList.Count - 1; i++)
            {
                var current = priceList[i];
                var next = priceList[i + 1];

                if (weight > current.Weight && weight <= next.Weight)
                    return next.Price;
            }

            // اگر وزن بیشتر از بیشترین مقدار وارد شده توسط مدیر است،
            // آخرین ردیف را برای هر بازه تکرار کن (مثل ۲۰ کیلو به بالا)
            var last = priceList.Last();
            int price = 0;
            while (weight > 0)
            {
                if (weight > last.Weight)
                {
                    price += last.Price;
                    weight -= last.Weight;
                }
                else
                {
                    price += last.Price;
                    weight = 0;
                }
            }

            return price;
        }

        public async Task<bool> UpdatePrice(PostPrice postPrice)
        {
            var obj = await _masterPrice.UpdateAsync(postPrice);

            return obj != null;
        }
       
    
    }
}
