using Azure.Core;
using Core.Service.Interface.Deliverd;
using Domain.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Service.Services.Delivery
{
    public class AloPeylServices : IDelivery
    {
        private readonly AloPeykClient _client;
        public AloPeylServices(string token, bool sandbox = true)
        {
            var baseUrl = sandbox
                ? "https://sandbox-api.alopeyk.com/api/v2/"
                : "https://api.alopeyk.com/api/v2/";

            _client = new AloPeykClient(baseUrl, token);
        }
        public async Task<string> CreateOrderAsync(OrderRequest request)
        {
            var data = new
            {
                transport_type = request.TransportType,
                addresses = new[]
                            {
                    new { lat = request.OriginLat, lng = request.OriginLng, address = request.OriginAddress, name = request.SenderName, phone = request.SenderPhone },
                    new { lat = request.DestinationLat, lng = request.DestinationLng, address = request.DestinationAddress, name = request.ReceiverName, phone = request.ReceiverPhone }
                },
                description = request.Description,
                pay_at_origin = request.PayAtOrigin
            };

            return await _client.PostAsync("orders", data);
        }

        public async Task<decimal> GetPriceAsync(double originLat, double originLng, double destLat, double destLng)
        {
            var data = new
            {
                transport_type = "motorbike",
                addresses = new[]
                     {
                    new { lat = originLat, lng = originLng, address = "Origin" },
                    new { lat = destLat, lng = destLng, address = "Destination" }
                }
            };

            var response = await _client.PostAsync("orders/price", data);
            var result = JsonDocument.Parse(response);

            return result.RootElement.GetProperty("price").GetDecimal();
        }

        public async Task<OrderStatus> TrackOrderAsync(string orderId)
        {
            var response = await _client.GetAsync($"orders/{orderId}/tracking");
            var result = JsonDocument.Parse(response);

            return new OrderStatus
            {
                OrderId = orderId,
                Status = result.RootElement.GetProperty("status").GetString(),
                LastUpdate = System.DateTime.UtcNow
            };
        }
    }
}
