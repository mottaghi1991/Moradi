using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Delivery
{
    public class AloPeykAddress
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }
        [JsonPropertyName("lng")]
        public double Lng { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; } // "origin" یا "destination"
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; } // توضیحات (اختیاری)
        [JsonPropertyName("person_fullname")]
        public string Person_Fullname { get; set; }
        [JsonPropertyName("person_phone")]
        public string Person_Phone { get; set; }
    }
}
