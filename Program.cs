using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.EntityFrameworkCore;
using UrlShortner.Data;
using UrlShortner.Features;

namespace UrlShortner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure for Render deployment
            var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(int.Parse(port));
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Get connection string from environment variable or fallback to appsettings
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                ?? builder.Configuration.GetConnectionString("DefaultConnection");

            //Db Context
            builder.Services.AddDbContext<UrlShortnerDbContext>(options =>
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.CommandTimeout(30); // 2 minutes timeout
                }));
            builder.Services.AddScoped<CreateShortUrlHandler>();
            builder.Services.AddScoped<RedirectUrlHandler>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            var app = builder.Build();



            // Ensure database is created -- safety, already exists
            //using (var scope = app.Services.CreateScope())
            //{
            //    var context = scope.ServiceProvider.GetRequiredService<UrlShortnerDbContext>();
            //    context.Database.EnsureCreated();
            //}

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
