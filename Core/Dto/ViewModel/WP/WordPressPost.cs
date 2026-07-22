using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.WP
{
    public class WordPressPost
    {
        public int Id { get; set; }
        public WordPressRendered Title { get; set; }
        public WordPressRendered Excerpt { get; set; }
        public string Link { get; set; }
        [JsonPropertyName("featured_media")]
        public int FeaturedMedia { get; set; }
        [JsonPropertyName("_embedded")]
        public WordPressEmbedded? Embedded { get; set; }
        [JsonIgnore]
        public string Image { get; set; }
    }
    public class WordPressEmbedded
    {
        [JsonPropertyName("wp:featuredmedia")]
        public List<WordPressMedia>? WpFeaturedMedia { get; set; }
    }
    public class WordPressMedia
    {
        [JsonPropertyName("source_url")]
        public string? SourceUrl { get; set; }
    }
    public class WordPressRendered { public string Rendered { get; set; } }
}
