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
                    var count = _context.ShortUrls.Count();
                    Console.WriteLine($"Current number of short URLs in the database: {count}");
                    isUnique = !_context.ShortUrls.Any(x => x.short_code == shortCode);
                }
                catch (Exception ex)
                {
                    // Let's log what's actually going wrong
                    Console.WriteLine($"Database query failed: {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    throw; // Let's not assume it's unique, something's wrong with our DB connection
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

        //public string Handle(string originalUrl)
        //{
        //    var existingUrl = _context.ShortUrls.FirstOrDefault(x => x.original_url == originalUrl);

        //    if (existingUrl != null)
        //    {
        //        return existingUrl.short_code; // Return existing code
        //    }
        //    string shortCode;
        //    bool isUnique = false;

        //    do
        //    {
        //        shortCode = Guid.NewGuid().ToString().Substring(0, 7);

        //        try
        //        {
        //            isUnique = !_context.ShortUrls.Any(x => x.short_code == shortCode);
        //        }
        //        catch (Exception)
        //        {
        //            // If uniqueness check fails due to timeout, assume it's unique and let DB handle it
        //            isUnique = true;
        //        }
        //    } while (!isUnique);

        //    var shortUrl = new ShortUrl
        //    {
        //        short_code = shortCode,
        //        original_url = originalUrl
        //    };

        //    _context.ShortUrls.Add(shortUrl);

        //    try
        //    {
        //        _context.SaveChanges();
        //    }
        //    catch (Exception)
        //    {
        //        // Clear the change tracker if save fails
        //        _context.ChangeTracker.Clear();
        //        throw;
        //    }

        //    return shortCode;
        //}
    }
}
