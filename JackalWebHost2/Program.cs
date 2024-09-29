using System;
using JackalWebHost2.Data;
using JackalWebHost2.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace JackalWebHost2;

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
            
        // builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
        builder.Services.AddControllers(opt =>
        {
            opt.SuppressAsyncSuffixInActionNames = true;
        }).AddNewtonsoftJson(jsonOpt =>
        {
            jsonOpt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonOpt.SerializerSettings.DateFormatString = "dd.MM.yyyy";
        });

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

        var allOrigins = "AllOrigins";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: allOrigins,
                act =>
                {
                    act.AllowAnyOrigin();
                    act.AllowAnyMethod();
                    act.AllowAnyHeader();
                });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //app.UseHsts();
        }

        app.UseCors(allOrigins);
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