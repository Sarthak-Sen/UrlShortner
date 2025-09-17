using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Models
{
    public class ShortUrl
    {
        public int id { get; set; }

        [Required]
        [StringLength(10)]
        public string short_code { get; set; } = string.Empty;

        [Required]
        public string original_url { get; set; } = string.Empty;

        public DateTime created_at { get; set; } = DateTime.UtcNow;

        public int click_count { get; set; } = 0;
    }
}
