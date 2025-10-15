using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Shop;

namespace Core.Service.Interface.Shop
{
    public interface IOrderItem
    {
        Task<int> GetSumOrderItembyBatchId(int BatchId);
        Task<IEnumerable<OrderItem>> GetAllByBatchId(int BatchId);
        Task<IEnumerable<OrderItem>> GetAllPaidItemByBatchId(int BatchId);
    }
}
