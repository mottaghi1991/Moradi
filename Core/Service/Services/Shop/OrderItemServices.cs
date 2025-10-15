using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Service.Interface.Shop;
using Data.MasterInterface;
using Domain;
using Domain.Shop;
using Microsoft.EntityFrameworkCore;

namespace Core.Service.Services.Shop
{
    public class OrderItemServices:IOrderItem
    {
        private readonly IMaster<OrderItem> _master;

        public OrderItemServices(IMaster<OrderItem> master)
        {
            _master = master;
        }

        public async Task<int> GetSumOrderItembyBatchId(int BatchId)
        {
            return await _master.GetAllAsQueryable()
                .Include(a=>a.Order)
                .Where(a=>a.ProductBatchId== BatchId&&a.Order.Status!=OrderStatus.Pending)
                .SumAsync(o => (int?)o.Quantity) ?? 0;
        }

        public async Task<IEnumerable<OrderItem>> GetAllByBatchId(int BatchId)
        {
            return await _master.GetAllEfAsync(a => a.ProductBatchId == BatchId);
        }

        public async Task<IEnumerable<OrderItem>> GetAllPaidItemByBatchId(int BatchId)
        {
            return  _master.GetAllAsQueryable()
                .Include(o => o.Order)
                .Where(o => o.ProductBatchId == BatchId && o.Order.Status!=OrderStatus.Pending);
        }
    }
}
