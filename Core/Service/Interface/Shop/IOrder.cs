using Domain.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core.Service.Interface.Shop
{
    public interface IOrder
    {
        Task<bool> FillOrder(Cart cart, int UserId, int AddressId, int sendPrice);
        Task<bool> Insert(Order order);
        Task<bool> Update(Order order);
        Task<Order> GetOrderByUserId(int userId);
        Task<bool> UpdateToFirstPay(int orderId, string Aauthority);
        Task<Order> GetOrderById(int orderId);
        Task<Order> GetOrderByAutority(string Autority);
        Task<bool> UpdateToFinaltPay(Order order);
    }
}
