using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Delivery
{
    public class AloPeykOrderRequest
    {
        [JsonPropertyName("transport_type")]
        public string Transport_Type { get; set; } = "motor_taxi"; // یا "motor_van" و...
        [JsonPropertyName("has_return")]
        public bool Has_Return { get; set; } = false;
        [JsonPropertyName("cashed")]
        public bool Cashed { get; set; } = false; // یعنی پرداخت در محل
        [JsonPropertyName("pay_at_dest")]
        public bool Pay_At_Dest { get; set; } = false;
        [JsonPropertyName("addresses")]
        public List<AloPeykAddress> Addresses { get; set; } = new();
        [JsonPropertyName("extra_params")]
        public AloPeykExtraParams Extra_Params { get; set; }
    }
}
