namespace JackalWebHost2.Infrastructure.Middleware;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCorsHeaders(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorsHeadersMiddleware>();
    }
}