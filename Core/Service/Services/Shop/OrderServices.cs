using Core.Dto.ViewModel.Dr.DietVM;
using Core.Dto.ViewModel.main;
using Core.Interface.Sms;
using Core.Interface.Store;
using Core.Service.Interface.Shop;
using Data;
using Data.MasterInterface;
using Domain;
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
        private readonly IProduct _product;
        protected MyContext _ctx;


        public OrderServices(IMaster<Order> master, ICart cart, IProduct product, MyContext ctx)
        {
            _master = master;
            _cart = cart;
            _product = product;
            _ctx = ctx;
        }

        public async Task<Order> FillOrder(Cart cart, int UserId, int AddressId, int sendPrice, DeliveryMethod method)
        {
         var action=  await _master.BeginTransactionAsync();

            try
            {
                var order = new Order()
                {
                    UserId = UserId,
                    OrderDate = DateTime.UtcNow,
                    Amount = cart.Items.Sum(a => a.Product.ProductBatches.FirstOrDefault(a=>a.IsActive==true).Price*a.Quantity),
                    DeliveryMethod = method,
                    Status = Domain.OrderStatus.Pending,
                    ShippingAddressId = AddressId,
                    PaymentAuthority = null,
                    PaymentDate = null,
                    PaymentRefId = null,
                    SendPrice = sendPrice,
                    TotalAmount = (cart.Items.Sum(a => a.Product.ProductBatches.FirstOrDefault(a=>a.IsActive).Price*a.Quantity) + sendPrice),
                    OrderItems = new List<OrderItem>()
                };

                foreach (var cartitem in cart.Items)
                {
                    var orderitem = new OrderItem()
                    {
                        ProductId = cartitem.ProductId,
                        Quantity = cartitem.Quantity,
                        UnitPrice = cartitem.UnitPrice,
                        ProductBatchId = cartitem.ProductBatchId,

                    };
                    order.OrderItems.Add(orderitem);
                }
                var result = await Insert(order);
                if (!result)
                {
                    await action.RollbackAsync();
                    return null;
                }

                // ➋ بررسی تمام شدن موجودی برای هر Batch (بدون تغییر Stock)
                foreach (var item in order.OrderItems)
                {
                    // استفاده از سرویس گزارش‌گیری برای محاسبه RemainingCount
                    var report = await _product.GetBatchUsageAsync(item.ProductBatchId);
                    if (report == null) continue;

                    // اگر موجودی تموم شد فقط Batch غیر فعال بشه
                    if (report.RemainingCount <= 0)
                    {
                        var batch = await _product.GetProductBatchById(item.ProductBatchId);
                            

                        if (batch != null && batch.IsActive)
                        {
                            batch.IsActive = false;
                           var updateres=await _product.UpdateBatchId(batch);
                           if (!updateres)
                           {
                               await action.RollbackAsync();
                               return null;
                            }
                        }
                    }
                }

               

                var removeredult=await _cart.RemoveUserCart(UserId);
                if(!removeredult)
                {
                    await action.RollbackAsync();
                    return null;
                }
                await action.CommitAsync();
                return order;
            }
            catch (Exception ex) {
                await action.RollbackAsync();
                return null;
            }
           
                 
        }

        public async Task<IEnumerable<Order>> GetAllOrder()
        {
            return await _master.GetAllAsQueryable().Include(a=>a.User).Include(a=>a.ShippingAddress)
                .ThenInclude(a=>a.province)
                .Include(a => a.OrderItems)
                .ThenInclude(i => i.Product).OrderByDescending(a=>a.PaymentDate)
                             .ToListAsync();
        }
        public async Task<Paging<Order>> GetPagedOrdersAsync(int? userId, string paymentStatus, string fullName, string mobile, int pageNumber, int pageSize)
        {
            var query = _master.GetAllAsQueryable()
                .Include(a => a.User)
                .Include(a => a.ShippingAddress)
                    .ThenInclude(a => a.province)
                .Include(a => a.OrderItems)
                    .ThenInclude(i => i.Product)
                .OrderByDescending(a => a.PaymentDate)
                .AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(paymentStatus) && int.TryParse(paymentStatus, out var statusValue))
            {
                var status = (OrderStatus)statusValue;
                query = query.Where(a => a.Status == status);
            }

            // 🔸 فیلتر بر اساس نام کاربر (Case-insensitive)
            if (!string.IsNullOrWhiteSpace(fullName))
                query = query.Where(a => a.User.FullName.Contains(fullName));
            if (!string.IsNullOrWhiteSpace(mobile))
                query = query.Where(a => a.User.UserName.Contains(mobile));
            // 🔸 پیجینگ
            var totalCount = await query.CountAsync();
            var data = await query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();
            return new Paging<Order>
            {
                bjects = data,
                TotalCount = totalCount,
                pageNumber = pageNumber,
                pageSize = pageSize,
                fullName = fullName,
                mobile = mobile,
                paymentStatus = paymentStatus,
                userId = userId

            };
          
        }

        public async Task<IEnumerable<Order>> GetAllOrderByUserId(int userId)
        {
            return await _master.GetAllAsQueryable().Include(a=>a.User).Include(a => a.OrderItems).ThenInclude(i => i.Product)
                   .Where(c => c.UserId == userId).OrderByDescending(a=>a.PaymentDate).ToListAsync();
        }

        public async Task<Order> GetOrderByAutority(string Autority)
        {
            var obj =  _master.GetAllAsQueryable(a => a.PaymentAuthority == Autority)
                .Include(a=>a.OrderItems)
                .Include(a=>a.ShippingAddress)
                .Include(a=>a.User);
            return obj.FirstOrDefault();
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            var obj = await _master.GetAllAsQueryable()
                .Include(a=>a.ShippingAddress)
                .Include(a => a.User)
                .Include(a => a.OrderItems)
                .ThenInclude(i => i.Product)
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
            var result= _ctx.SaveChanges();
        if(result==0)
                return false;

        return true;
        }

    }
}
