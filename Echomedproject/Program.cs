using Echomedproject.BLL.Interfaces;
using Echomedproject.BLL.Repositories;
using Echomedproject.DAL.Contexts;
using Echomedproject.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace Echomedproject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Define CORS policy name
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register DbContext
            builder.Services.AddDbContext<EckomedDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Register services
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200") // Angular URL
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials(); // Optional if you're using cookies
                    });
            });

            // Identity configuration
            builder.Services.AddIdentity<Users, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<EckomedDbContext>()
            .AddDefaultTokenProviders();



            builder.Services.AddAuthentication();
            builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/api/account/login";
        options.AccessDeniedPath = "/api/account/access-denied";
        options.Cookie.Name = "MyAppAuthCookie";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None; // Important for cross-origin
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRouting();
            app.UseCors("AllowAngularApp");


            app.UseAuthentication();             // Enable authentication
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // API routes
            });
            app.UseCors("AllowLocalhost");

            // Fallback to Angular index.html for client-side routing

            app.Run();
        }
    }
}
