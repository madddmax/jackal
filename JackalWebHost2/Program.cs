using FluentValidation;
using FluentValidation.AspNetCore;
using Jackal.Core;
using JackalWebHost2.Controllers.Hubs;
using JackalWebHost2.Data;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Data.Repositories;
using JackalWebHost2.Infrastructure;
using JackalWebHost2.Infrastructure.Auth;
using JackalWebHost2.Infrastructure.Middleware;
using JackalWebHost2.Models;
using JackalWebHost2.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        if (app.Environment.IsDevelopment() ||
            app.Environment.IsStaging())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(CorsDefaults.AllOrigins);
        app.UseStaticFiles();
        app.UseCorsHeaders();

        app.UseRouting();

        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHub<GameHub>("/gamehub");
        app.MapControllers();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddSignalR().AddNewtonsoftJsonProtocol(jsonOpt =>
        {
            var enumConverter = new StringEnumConverter();
            jsonOpt.PayloadSerializerSettings.Converters.Add(enumConverter);
            jsonOpt.PayloadSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonOpt.PayloadSerializerSettings.DateFormatString = "dd.MM.yyyy";
        }); 

        services
            .AddControllers(opt =>
            {
                opt.SuppressAsyncSuffixInActionNames = true;
                opt.Filters.Add<ValidationFilter>();
                opt.Filters.Add<BusinessExceptionFilter>();
            })
            .AddNewtonsoftJson(jsonOpt =>
            {
                var enumConverter = new StringEnumConverter();
                jsonOpt.SerializerSettings.Converters.Add(enumConverter);
                jsonOpt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                jsonOpt.SerializerSettings.DateFormatString = "dd.MM.yyyy";
            });

        services
            .AddSwaggerGen()
            .AddMemoryCache()
            .AddCors(options =>
            {
                options.AddPolicy(name: CorsDefaults.AllOrigins,
                    act =>
                    {
                        act.WithOrigins(
                            "http://localhost:5130",
                            "http://localhost:5173",
                            "http://116.203.101.2",
                            "http://jackal2.online",
                            "http://jackal.team",
                            "https://jackal.team"
                        );
                        act.AllowAnyMethod();
                        act.AllowAnyHeader();
                        act.AllowCredentials();
                    });
            });

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(AuthDefaults.FastAuthScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthDefaults.Issuer,
                    ValidateAudience = true,
                    ValidAudience = AuthDefaults.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthDefaults.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/gamehub")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        services
            .AddAuthorization(options =>
            {
                options.AddPolicy(AuthDefaults.FastAuthPolicy, policy => policy
                    .AddAuthenticationSchemes(AuthDefaults.FastAuthScheme)
                    .RequireAuthenticatedUser());
            });

        services
            .AddValidatorsFromAssemblyContaining<Program>()
            .AddFluentValidationAutoValidation();

        services.AddHostedService<ActiveGamesPollingService>();

        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IDrawService, DrawService>();
        services.AddScoped<IMapService, MapService>();
        services.AddScoped<ILobbyService, LobbyService>();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrEmpty(connectionString))
        {
            services.AddDbContext<JackalDbContext>(options => options.UseNpgsql(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();
            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IGamePlayerRepository, GamePlayerRepository>();
        }
        else
        {
            services.AddScoped<IUserRepository, UserRepositoryInMemory>();
            services.AddScoped<IGameRepository, GameRepositoryStub>();
            services.AddScoped<IGamePlayerRepository, GamePlayerRepositoryStub>();
        }

        services.AddSingleton<IStateRepository<Game>, StateRepositoryInMemory<Game>>();
        services.AddSingleton<IStateRepository<NetGameSettings>, StateRepositoryInMemory<NetGameSettings>>();
        services.AddScoped<ILobbyRepository, LobbyRepositoryInMemory>();
    }
}