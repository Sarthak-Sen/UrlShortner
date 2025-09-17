using Microsoft.EntityFrameworkCore;
using UrlShortner.Data;

namespace UrlShortner.Features
{
    public class RedirectUrlHandler
    {
        private readonly UrlShortnerDbContext _context;

        public RedirectUrlHandler(UrlShortnerDbContext context)
        {
            _context = context;
        }

        public string? Handle(string shortCode)
        {
            var shortUrl = _context.ShortUrls.FirstOrDefault(x => x.short_code == shortCode);

            if (shortUrl != null)
            {
                shortUrl.click_count++;
                _context.SaveChanges();

                return shortUrl.original_url;
            }

            return null;
        }
    }
}
