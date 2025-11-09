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
            var url = $"https://drmoradi-diet.com/blogs/wp-json/wp/v2/posts?per_page={count}";
            var json = await _httpClient.GetStringAsync(url);

            var posts = JsonSerializer.Deserialize<List<WordPressPost>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<WordPressPost>();

            // اضافه کردن Header برای اطمینان از پاسخ
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (var post in posts)
            {
                Console.WriteLine($"Post {post.Id} - FeaturedMedia: {post.FeaturedMedia}");

                try
                {
                    if (post.FeaturedMedia > 0)
                    {
                        string[] possibleUrls =
                        {
                    $"https://drmoradi-diet.com/wp-json/wp/v2/media/{post.FeaturedMedia}",          // بدون blogs
                    $"https://drmoradi-diet.com/blogs/wp-json/wp/v2/media/{post.FeaturedMedia}"     // با blogs
                };

                        string? image = null;

                        foreach (var mediaUrl in possibleUrls)
                        {
                            try
                            {
                                var mediaJson = await _httpClient.GetStringAsync(mediaUrl);
                                using var doc = JsonDocument.Parse(mediaJson);

                                if (doc.RootElement.TryGetProperty("source_url", out var source))
                                {
                                    image = source.GetString();
                                    if (!string.IsNullOrWhiteSpace(image)) break; // اگر جواب گرفتیم از حلقه خارج شو
                                }
                                else if (doc.RootElement.TryGetProperty("guid", out var guid) &&
                                         guid.TryGetProperty("rendered", out var rendered))
                                {
                                    image = rendered.GetString();
                                    if (!string.IsNullOrWhiteSpace(image)) break;
                                }
                            }
                            catch
                            {
                                // رد شو، بعدی رو تست کن
                            }
                        }

                        post.Image = image ?? "/img/no-image.jpg";
                    }
                    else
                    {
                        post.Image = "/img/no-image.jpg";
                    }
                }
                catch
                {
                    post.Image = "/img/no-image.jpg";
                }
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
