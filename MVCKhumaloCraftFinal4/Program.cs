using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MVCKhumaloCraftFinal4.Data;
using MVCKhumaloCraftFinal4.Models;

namespace MVCKhumaloCraftFinal2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MVCKhumaloCraftFinal4Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MVCKhumaloCraftFinal4Context") ?? throw new InvalidOperationException("Connection string 'MVCKhumaloCraftFinal4Context' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register IHttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            // Add session services
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Configure cookie authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Home/Login";
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession(); // Ensure this is before UseAuthentication() and UseAuthorization()

            app.UseAuthentication();
            app.UseAuthorization();

            // Custom admin route
            app.MapControllerRoute(
                name: "admin",
                pattern: "Admin/{action=Login}/{id?}",
                defaults: new { controller = "Admins", action = "Login" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Seed admin user
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbContext = services.GetRequiredService<MVCKhumaloCraftFinal4Context>();
                    dbContext.Database.Migrate();
                    SeedAdmin(dbContext);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while seeding the database.");
                    Console.WriteLine(ex.Message);
                }
            }
            app.Run();
        }

        private static void SeedAdmin(MVCKhumaloCraftFinal4Context context)
        {
            if (context.Admin.Any())
            {
                return; // Database has been seeded
            }

            // Add admin user
            var admin = new Admin
            {
                adminEmail = "admin@gmail.com",
                adminPassword = "admin123" // Replace with your desired admin password
            };
            context.Admin.Add(admin);
            context.SaveChanges();
        }
    }
}
