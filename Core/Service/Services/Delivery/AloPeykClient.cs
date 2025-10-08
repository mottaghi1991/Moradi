using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Service.Services.Delivery
{
    public class AloPeykClient
    {
        private readonly HttpClient _httpClient;

        public AloPeykClient(string baseUrl, string token)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            _httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        }
        public async Task<string> GetAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
           public async Task<string> PostAsync<T>(string endpoint, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
