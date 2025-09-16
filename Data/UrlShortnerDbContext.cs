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
                entity.ToTable("short_urls"); // Map to the correct table name
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ShortCode).IsUnique();
                entity.Property(e => e.ShortCode).IsRequired().HasMaxLength(10);
                entity.Property(e => e.OriginalUrl).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.ClickCount).HasDefaultValue(0);

                // Map property names to column names
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ShortCode).HasColumnName("short_code");
                entity.Property(e => e.OriginalUrl).HasColumnName("original_url");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.ClickCount).HasColumnName("click_count");
            });
        }
    }
}
