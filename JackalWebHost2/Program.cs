using JackalWebHost2.Data;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Data.Repositories;
using JackalWebHost2.Infrastructure;
using JackalWebHost2.Middleware;
using JackalWebHost2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace JackalWebHost2;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);

        var app = builder.Build();
        ConfigurePipeline(app);
        await app.RunAsync();
    }

    private static void ConfigurePipeline(WebApplication app)
    {
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

        app.UseCors(CorsDefaults.AllOrigins);
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
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        // Add services to the container.
        var services = builder.Services;
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services
            .AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString))
            .AddDatabaseDeveloperPageExceptionFilter()
            .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services
            .AddControllers(opt =>
            {
                opt.SuppressAsyncSuffixInActionNames = true;
            })
            .AddNewtonsoftJson(jsonOpt =>
            {
                jsonOpt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                jsonOpt.SerializerSettings.DateFormatString = "dd.MM.yyyy";
            });

        services
            .AddDistributedMemoryCache()
            .AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            })
            .AddCors(options =>
            {
                options.AddPolicy(name: CorsDefaults.AllOrigins,
                    act =>
                    {
                        act.AllowAnyOrigin();
                        act.AllowAnyMethod();
                        act.AllowAnyHeader();
                    });
            });

        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IDrawService, DrawService>();
        
        services.AddScoped<IGameStateRepository, InMemoryGameStateRepository>();
    }
}