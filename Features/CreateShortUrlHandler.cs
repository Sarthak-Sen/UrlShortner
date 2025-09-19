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
                return existingUrl.short_code;
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Database query failed: {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    throw;
                }
            } while (!isUnique);

            var shortUrl = new ShortUrl
            {
                short_code = shortCode,
                original_url = originalUrl
            };

            _context.ShortUrls.Add(shortUrl);
            _context.SaveChanges();

            return shortCode;
        }
    }
}
