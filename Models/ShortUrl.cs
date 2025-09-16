using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Models
{
    public class ShortUrl
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string ShortCode { get; set; } = string.Empty;

        [Required]
        public string OriginalUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int ClickCount { get; set; } = 0;
    }
}
