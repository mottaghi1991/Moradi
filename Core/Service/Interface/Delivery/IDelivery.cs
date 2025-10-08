using Domain.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Deliverd
{
    public interface IDelivery
    {
        Task<decimal> GetPriceAsync(double originLat, double originLng, double destLat, double destLng);
        Task<string> CreateOrderAsync(OrderRequest orderRequest);
        Task<OrderStatus> TrackOrderAsync(string orderId);
    }
}
