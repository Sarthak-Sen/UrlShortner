using Microsoft.EntityFrameworkCore;
using UrlShortner.Models;

namespace UrlShortner.Data
{
    public class UrlShortnerDbContext : DbContext
    {
        public UrlShortnerDbContext(DbContextOptions<UrlShortnerDbContext> options) : base(options)
        {
        }

        public DbSet<ShortUrl> ShortUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortUrl>(entity =>
            {
                entity.ToTable("short_urls");
                entity.HasKey(e => e.id);
                entity.HasIndex(e => e.short_code).IsUnique();
                entity.Property(e => e.short_code).IsRequired().HasMaxLength(10);
                entity.Property(e => e.original_url).IsRequired();
                entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.click_count).HasDefaultValue(0);
            });
        }
    }
}
