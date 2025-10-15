using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Delivery
{
    public class AloPeykOrderObject
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("invoice_number")]
        public string InvoiceNumber { get; set; }

        [JsonPropertyName("price")]
        public int Price { get; set; }

        [JsonPropertyName("final_price")]
        public int FinalPrice { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("order_token")]
        public string OrderToken { get; set; }

        [JsonPropertyName("customer_id")]
        public int CustomerId { get; set; }

        // ⚠️ این کلید در JSON نیست ولی خودت می‌سازی برای راحتی.
        // با استفاده از OrderToken می‌سازیمش
        [JsonIgnore]
        public string TrackingUrl
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(OrderToken))
                    return $"https://alopeyk.com/order/{OrderToken}";
                return null;
            }
        }
    }
}
