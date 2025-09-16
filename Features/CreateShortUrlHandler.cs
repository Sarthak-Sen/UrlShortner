using Microsoft.EntityFrameworkCore;
using UrlShortner.Data;
using UrlShortner.Models;

namespace UrlShortner.Features
{
    public class CreateShortUrlHandler
    {

        private readonly UrlShortnerDbContext _context;

        public CreateShortUrlHandler(UrlShortnerDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(string originalUrl)
        {
            var existingUrl = await _context.ShortUrls.FirstOrDefaultAsync(x => x.OriginalUrl == originalUrl);

            if (existingUrl != null)
            {
                return existingUrl.ShortCode; // Return existing code
            }
            string shortCode;
            bool isUnique = false;

            do
            {
                shortCode = Guid.NewGuid().ToString().Substring(0, 7);

                try
                {
                    isUnique = !await _context.ShortUrls.AnyAsync(x => x.ShortCode == shortCode);
                }
                catch (Exception)
                {
                    // If uniqueness check fails due to timeout, assume it's unique and let DB handle it
                    isUnique = true;
                }
            } while (!isUnique);

            var shortUrl = new ShortUrl
            {
                ShortCode = shortCode,
                OriginalUrl = originalUrl
            };

            _context.ShortUrls.Add(shortUrl);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Clear the change tracker if save fails
                _context.ChangeTracker.Clear();
                throw;
            }

            return shortCode;
        }
    }
}
