using System;
using JackalWebHost2.Data;
using JackalWebHost2.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JackalWebHost2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //var config = builder.Configuration;

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            //builder.Services.AddAuthentication()
            //   .AddGoogle(options =>
            //   {
            //       IConfigurationSection googleAuthNSection = config.GetSection("Authentication:Google");
            //       options.ClientId = googleAuthNSection["ClientId"];
            //       options.ClientSecret = googleAuthNSection["ClientSecret"];
            //   });

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var devOrigins = "DevOrigins";
            var prodOrigins = "ProdOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: devOrigins,
                    act =>
                    {
                        act.WithOrigins(new[]{"http://localhost:5130"});
                        act.AllowAnyHeader();
                        act.AllowAnyMethod();
                        act.AllowCredentials();
                    });
                options.AddPolicy(name: prodOrigins,
                    act =>
                    {
                        act.WithOrigins("http://116.203.101.2");
                        act.AllowAnyHeader();
                        act.AllowAnyMethod();
                        act.AllowCredentials();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseCors(devOrigins);
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
                app.UseCors(prodOrigins);
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseOptions();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Game}/{action=Index}/{id?}"
            );
            app.Run();
        }
    }
}