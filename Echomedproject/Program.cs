using Echomedproject.BLL.Interfaces;
using Echomedproject.BLL.Repositories;
using Echomedproject.DAL.Contexts;
using Echomedproject.DAL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Define CORS policy name
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Register DbContext
        builder.Services.AddDbContext<EckomedDbContext>(options =>
        {
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.CommandTimeout(120); // Timeout in seconds (e.g., 2 minutes)
                });
        });


        // Register services
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IEmailSender, EmailSender>();

        // CORS configuration
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularApp",
                policy =>
                {
                    policy.WithOrigins("http://localhost:4200") // Angular URL
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // Allow credentials (cookies)
                });
        });

        builder.Services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = 5242880000;
        });

        // Identity configuration
        builder.Services.AddIdentity<Users, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<EckomedDbContext>()
        .AddDefaultTokenProviders();

        // Authentication configuration
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "MyAppAuthCookie";
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.None;

            options.LoginPath = "/api/account/userlogin";
            options.LogoutPath = "/api/account/logout";
            options.AccessDeniedPath = "/api/account/access-denied";

            options.ExpireTimeSpan = TimeSpan.FromDays(7);
            options.SlidingExpiration = true;

            // 🔐 Override default redirect behavior for APIs
            options.Events.OnRedirectToLogin = context =>
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            };

            options.Events.OnRedirectToAccessDenied = context =>
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                }
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            };
        });








        // This is the key part to make sure your scheme is default
        builder.Services.Configure<AuthenticationOptions>(options =>
        {
            options.DefaultScheme = "MyCookieAuth";
        });


        builder.Services.AddEndpointsApiExplorer();
        var app = builder.Build();

        // Initialize roles
        var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "User", "DataEntry", "Pharmacy" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // HTTP request pipeline configuration
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            await next();
        });

        app.UseRouting();
        app.UseCors("AllowAngularApp");

        // Authentication and Authorization middleware
        app.UseAuthentication();
        app.UseAuthorization();

        // HTTPS and Static Files middleware
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        // API routes
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // API routes
        });

        app.Run();
    }
}
