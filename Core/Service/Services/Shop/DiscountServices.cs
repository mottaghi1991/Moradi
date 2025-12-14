using Core.Dto;
using Core.Service.Interface.Shop;
using Data.MasterInterface;
using Data.Migrations;
using Domain.Shop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Shop
{
    public class DiscountServices : IDiscount
    {
        private readonly IMaster<Discount> _master;

        public DiscountServices(IMaster<Discount> master)
        {
            _master = master;
        }

        public async Task<bool> DeactiveCode(int OrderId)
        {
           var obj=await _master.GetAllEfAsync(a=>a.OrderId == OrderId);
            var item=obj.FirstOrDefault();
            item.IsUsed = true;
           return await update(item)!=null;


        }

        public async Task<ServiceResponse> Delete(Discount discount)
        {
          
            discount.IsDeleted = true;
            discount.DeleteTime = DateTime.Now;
            var result = await _master.UpdateAsync(discount);
            if (result != null)
            {
                return new ServiceResponse()
                {
                    ErrorId = 0,
                    ErrorTitle = "عملیات با موفق انجام شد .",
                    Object = result
                };
            }
            return new ServiceResponse()
            {

                ErrorTitle = "عملیت با خطا مواجه گردید"
            };
        }

        public async Task<Discount> GetDiscountByCode(string code)
        {
            var obj = await _master.GetAllEfAsync(a => a.Code == code&&!a.IsDeleted&&!a.IsUsed&&a.OrderId==null);
            return obj.FirstOrDefault();
        }

        public async Task<Discount> GetDiscountById(int Id)
        {
            var obj= await _master.GetAllEfAsync(a => a.Id == Id);
            return obj.FirstOrDefault();
        }

        public async Task<IEnumerable<Discount>> GetDiscountsByStatus(bool? status)
        {
            if (status == null)
                return _master.GetAllAsQueryable();
            else
                return _master.GetAllAsQueryable(a => a.IsUsed == status);
        }

        public async Task<Discount> Insert(Discount discount)
        {
            discount.IsUsed = false;
            discount.Code = Core.Extention.CodeGenerator.DiscountGenerate();
            return await _master.InsertAsync(discount);
        }
        public async Task<ServiceResponse> update(Discount discount)
        {
            var oldDiscount =await GetDiscountById(discount.Id);
            if(oldDiscount.IsUsed)
            {
                return new ServiceResponse()
                {
                    ErrorTitle = "کد استفاده شده و قابل ویرایش نمی باشد ."
                };
            }
            oldDiscount.Percent = discount.Percent;
            oldDiscount.IsDeleted = discount.IsDeleted;
            var result = await _master.UpdateAsync(oldDiscount);
            if(result!=null)
            {
                return new ServiceResponse()
                {
                    ErrorId = 0,
                    ErrorTitle = "عملیات با موفق انجام شد .",
                    Object=result
                };
            }
            return new ServiceResponse()
            {

                ErrorTitle = "عملیت با خطا مواجه گردید"
            };
        }
    }
}
