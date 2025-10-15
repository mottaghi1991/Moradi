using Domain.Delivery;
using Domain.Shop;
using Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Deliverd
{
    public interface IDelivery
    {
        Task<AloPeykPriceResponse> GetPriceAsync(AloPeykPriceRequest request);
        Task<AloPeykOrderResponse> CreateOrderAsync(AloPeykOrderRequest request);
        Task<AloPeykOrderResponse?> GetOrderDetailAsync(int orderId);
        Task<bool> CreateAloPeykOrderAsync(Order order, ShippingAddres Destination, MyUser ReciveUser);

    }
}
