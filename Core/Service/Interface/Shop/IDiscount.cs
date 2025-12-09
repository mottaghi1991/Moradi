using Core.Dto;
using Domain.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Shop
{
    public interface IDiscount
    {
        Task<Discount> Insert(Discount discount);
        Task<ServiceResponse> update(Discount discount);
        Task<ServiceResponse> Delete(Discount discount);
        Task<Discount> GetDiscountById(int Id);
        Task<IEnumerable<Discount>> GetDiscountsByStatus(bool? status);

    }
}
