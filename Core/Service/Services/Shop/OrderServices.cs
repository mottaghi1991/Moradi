using Core.Service.Interface.Shop;
using Data.MasterInterface;
using Domain.Dr;
using Domain.Shop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Shop
{

    public class OrderServices : IOrder
    {
        private readonly IMaster<Order> _master;
        private readonly ICart _cart;

        public OrderServices(IMaster<Order> master, ICart cart)
        {
            _master = master;
            _cart = cart;
        }

        public async Task<bool> FillOrder(Cart cart, int UserId, int AddressId, int sendPrice)
        {
         var action=  await _master.BeginTransactionAsync();
            try
            {
                var order = new Order()
                {
                    UserId = UserId,
                    OrderDate = DateTime.UtcNow,
                    Amount = cart.Items.Sum(a => a.Product.Price*a.Quantity),

                    Status = Domain.OrderStatus.Pending,
                    ShippingAddressId = AddressId,
                    PaymentAuthority = null,
                    PaymentDate = null,
                    PaymentRefId = null,
                    SendPrice = sendPrice,
                    TotalAmount = (cart.Items.Sum(a => a.Product.Price*a.Quantity) + sendPrice),
                    OrderItems = new List<OrderItem>()
                };

                foreach (var cartitem in cart.Items)
                {
                    var orderitem = new OrderItem()
                    {
                        ProductId = cartitem.ProductId,
                        Quantity = cartitem.Quantity,
                        UnitPrice = cartitem.UnitPrice,

                    };
                    order.OrderItems.Add(orderitem);
                }
                var result = await Insert(order);
                if (!result)
                {
                    await action.RollbackAsync();
                    return false;
                }
               var removeredult=await _cart.RemoveUserCart(UserId);
                if(!removeredult)
                {
                    await action.RollbackAsync();
                    return false;
                }
                await action.CommitAsync();
                return true;
            }
            catch (Exception ex) {
                await action.RollbackAsync();
                return false;
            }
           
                 
        }

        public async Task<IEnumerable<Order>> GetAllOrder()
        {
            return await _master.GetAllAsQueryable().Include(a=>a.ShippingAddress).ThenInclude(a=>a.province).Include(a => a.OrderItems).ThenInclude(i => i.Product)
                             .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrderByUserId(int userId)
        {
            return await _master.GetAllAsQueryable().Include(a => a.OrderItems).ThenInclude(i => i.Product)
                   .Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<Order> GetOrderByAutority(string Autority)
        {
            var obj = await _master.GetAllEfAsync(a => a.PaymentAuthority == Autority);
            return obj.FirstOrDefault();
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            var obj = await _master.GetAllAsQueryable().Include(a=>a.ShippingAddress).Include(a => a.OrderItems).ThenInclude(i => i.Product)
              .Where(c => c.Id == orderId).ToListAsync();
            return obj.FirstOrDefault();
        }

        public async Task<Order> GetOrderByUserId(int userId)
        {
            var obj = await _master.GetAllAsQueryable().Include(a => a.OrderItems).ThenInclude(i => i.Product)
              .Where(c => c.UserId == userId).ToListAsync();
            return obj.FirstOrDefault();
        }

        public async Task<bool> Insert(Order order)
        {
            var obj= await _master.InsertAsync(order);
            return obj != null;
        }

        public async Task<bool> Update(Order order)
        {
            var obj= await _master.UpdateAsync(order);
            return obj!=null;
        }

        public async Task<bool> UpdateToFinaltPay(Order order)
        {
            var obj = await _master.UpdateAsync(order);
            return obj != null;
        }

        public async Task<bool> UpdateToFirstPay(int orderId, string Aauthority)
        {
            var obj =await GetOrderById(orderId);        
        if(obj == null)
                return false;
        obj.PaymentAuthority = Aauthority;
            var result=await Update(obj);
        if(!result)
                return false;

        return true;
        }

    }
}
