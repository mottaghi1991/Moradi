using Azure.Core;
using Core.Service.Interface.Deliverd;
using Domain.Delivery;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Service.Interface.Shop;
using Domain.Shop;
using Domain.User;

namespace Core.Service.Services.Delivery
{
    public class AloPeylServices : IDelivery
    {
        private readonly AloPeykSettings _settings;
        private readonly ILogger<AloPeylServices> _logger;
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://sandbox-api.alopeyk.com/api/v2/";
        private readonly string _token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOjM3MjAyLCJpc3MiOiJodHRwOi8vc2FuZGJveC1wYW5lbC5hbG9wZXlrLmNvbS9nZW5lcmF0ZS10b2tlbi8zNzIwMiIsImlhdCI6MTc1OTkxMzc3MywiZXhwIjo1MzU5OTEzNzczLCJuYmYiOjE3NTk5MTM3NzMsImp0aSI6IndBUU1rWll6ZVN6MjFuZW4ifQ.Gjb5Iwxk90OzkrY-NKyz-z9mzPJ59K8m6CY4XIAaCCM";
        private readonly IOrder _order;

        public AloPeylServices(HttpClient client, ILogger<AloPeylServices> logger, AloPeykSettings settings, IOrder order)
        {
            _client = client;
            _logger = logger;
            _settings = settings;
            _order = order;
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _token);
            _client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            _client.BaseAddress = new Uri(_baseUrl);

        }


        public async Task<AloPeykPriceResponse> GetPriceAsync(AloPeykPriceRequest request)
        {
            var body = new
            {
                transport_type = request.TransportType ?? "motorbike",
                addresses = new[]
                {
                    new {
                        type = "origin",
                        lat = request.FromLat,
                        lng = request.FromLng
                    },
                    new {
                        type = "destination",
                        lat = request.ToLat,
                        lng = request.ToLng
                    }
                }
            };
            string json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_baseUrl + "orders/price/calc", content);
            string result = await response.Content.ReadAsStringAsync();

            AloPeykPriceResponse? data;

            try
            {
                data = JsonSerializer.Deserialize<AloPeykPriceResponse>(
                    result,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            catch (Exception ex)
            {
                // اگه JSON خراب بود یا ساختار عوض شده بود
                return new AloPeykPriceResponse
                {
                    Status = "error",
                    Message = $"JSON parse error: {ex.Message}\nRaw: {result}"
                };
            }

            // اگر Deserialize موفق نشد (null برگشت)
            if (data == null)
            {
                return new AloPeykPriceResponse
                {
                    Status = "error",
                    Message = "Invalid or empty JSON response from AloPeyk API."
                };
            }

            // در صورت موفقیت
            data.Message = "Price retrieved successfully";
            return data;


        }

        public async Task<AloPeykOrderResponse> CreateOrderAsync(AloPeykOrderRequest request)
        {
            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await _client.PostAsync("orders", content);
            var body = await res.Content.ReadAsStringAsync();

            _logger.LogInformation("AloPeyk CreateOrder Response: {Body}", body);

            try
            {
                return JsonSerializer.Deserialize<AloPeykOrderResponse>(
                    body,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در Deserialize پاسخ CreateOrder");
                return null;
            }
        }

        public async Task<AloPeykOrderResponse> GetOrderDetailAsync(int orderId)
        {
            var res = await _client.GetAsync($"orders/{orderId}");
            var body = await res.Content.ReadAsStringAsync();

            _logger.LogInformation("AloPeyk GetOrderDetail ({OrderId}) Response: {Body}", orderId, body);

            return JsonSerializer.Deserialize<AloPeykOrderResponse>(
                body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }

        public async Task<bool> CreateAloPeykOrderAsync(Order order,ShippingAddres Destination,MyUser ReciveUser)
        {
            var origin = _settings.Origin;
            var req = new AloPeykOrderRequest
            {
                Transport_Type = "motorbike",
                Has_Return = false,
                Cashed = false,
                Pay_At_Dest = false,
                Addresses = new List<AloPeykAddress>
            {
                new AloPeykAddress
                {
                    Type = "origin",
                    Lat = origin.Lat,
                    Lng = origin.Lng,
                    Address = origin.Address,
                    Description = origin.Description,
                    Person_Fullname = origin.FullName,
                    Person_Phone = origin.Phone
                },
                new AloPeykAddress
                {
                    Type = "destination",
                    Lat = Destination.Latitude.Value,
                    Lng = Destination.Longitude.Value,
                    Address = Destination.AddressLine,
                    Description = $"تحویل سفارش {order.Id} به مشتری",
                    Person_Fullname = ReciveUser.FullName,
                    Person_Phone = ReciveUser.UserName
                }
            }
            };

            try
            {
                var aloResp = await CreateOrderAsync(req);

                if (aloResp?.Object != null)
                {
           

                    order.AloPeykOrderId = aloResp.Object.Id;
                    order.AloPeykTrackingUrl = aloResp.Object.TrackingUrl;
                    order.AloPeykStatus = aloResp.Object.Status;

                    await _order.Update(order);

                    _logger.LogInformation("سفارش Alopeyk ثبت شد: {OrderId} / {AloId}", order.Id, order.AloPeykOrderId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ایجاد سفارش Alopeyk برای OrderId={OrderId}", order.Id);
            }
            return false;
        }
    }
}
