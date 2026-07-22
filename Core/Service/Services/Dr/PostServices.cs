using Core.Dto.ViewModel.WP;
using Core.Service.Interface.Dr;
using Data.MasterInterface;
using Data.Migrations;
using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Service.Services.Dr
{
    public class PostServices : IPost
    {
        private readonly IMaster<Post> _master;
        private readonly HttpClient _httpClient;

        public PostServices(IMaster<Post> master, HttpClient httpClient)
        {
            _master = master;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Post>> GetAllPostPaging(int pageid,int number)
        {
            var obj = await _master.GetPagingAsync(pageid, number);
            return obj.OrderByDescending(a=>a.Id);
        }

        public async Task<Post> GetpostById(int id)
        {
            var obj= await _master.GetAllEfAsync(a=>a.Id==id);
            return obj.FirstOrDefault();
                }

        public async Task<IEnumerable<Post>> GetTopPost(int number)
        {
            var obj =await _master.GetAllEfAsync();
            return obj.Take(number);
        }

        public async Task<List<WordPressPost>> GetTopWordPressPost(int count)
        {
            var url = $"https://drmoradi-diet.com/blogs/wp-json/wp/v2/posts?per_page={count}&_embed=1";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            request.Headers.Accept.ParseAdd("application/json");

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var posts = JsonSerializer.Deserialize<List<WordPressPost>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<WordPressPost>();

            foreach (var post in posts)
            {
                post.Image = post.Embedded?.WpFeaturedMedia?.FirstOrDefault()?.SourceUrl
                             ?? "/img/no-image.jpg";
            }

            return posts;
        }



        public async Task<int> PostCount()
        {
            var obj = await _master.GetAllEfAsync();
            return obj.Count();
        }
    }
}
