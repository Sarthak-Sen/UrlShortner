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

        public string Handle(string originalUrl)
        {
            var existingUrl = _context.ShortUrls.FirstOrDefault(x => x.original_url == originalUrl);

            if (existingUrl != null)
            {
                return existingUrl.short_code; // Return existing code
            }
            string shortCode;
            bool isUnique = false;

            do
            {
                shortCode = Guid.NewGuid().ToString().Substring(0, 7);

                try
                {
                    isUnique = !_context.ShortUrls.Any(x => x.short_code == shortCode);
                }
                catch (Exception)
                {
                    // If uniqueness check fails due to timeout, assume it's unique and let DB handle it
                    isUnique = true;
                }
            } while (!isUnique);

            var shortUrl = new ShortUrl
            {
                short_code = shortCode,
                original_url = originalUrl
            };

            _context.ShortUrls.Add(shortUrl);

            try
            {
                _context.SaveChanges();
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
