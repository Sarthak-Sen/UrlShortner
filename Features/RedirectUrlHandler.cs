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

        public async Task<string?> Handle(string shortCode)
        {
            var shortUrl = await _context.ShortUrls.FirstOrDefaultAsync(x => x.ShortCode == shortCode);

            if (shortUrl != null)
            {
                shortUrl.ClickCount++;
                await _context.SaveChangesAsync();

                return shortUrl.OriginalUrl;
            }

            return null;
        }
    }
}
